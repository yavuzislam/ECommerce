using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Payment.BusinessLayer.Abstract;
using Payment.BusinessLayer.Concrete;
using Payment.BusinessLayer.Utilities.Services.MailServices;
using Payment.BusinessLayer.Utilities.Tool;
using Payment.DataAccessLayer.Abstract;
using Payment.DataAccessLayer.Concrete;
using Payment.DataAccessLayer.EntityFramework;
using Payment.EntityLayer.Concrete;
using Payment.WebApi.Hubs;
using Payment.WebApi.Mapping;
using Payment.WebUI.ValidationRules.RegisterValidationRules;
using Serilog;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog yapýlandýrmasý
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

// DbContext ve Identity yapýlandýrmasý
string connectionString = builder.Configuration.GetConnectionString("MsSqlConnection");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>(
    i => new EmailSenderService(
        builder.Configuration["EmailSender:Host"],
        builder.Configuration.GetValue<int>("EmailSender:Port"),
        builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
        builder.Configuration["EmailSender:Username"],
        builder.Configuration["EmailSender:Password"])
);


// AutoMapper yapýlandýrmasý
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<IProductDal, EfProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthService, AuthManager>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddScoped<IAddressDal, EfAddressDal>();
builder.Services.AddScoped<IAddressService, AddressManager>();

builder.Services.AddScoped<IWishlistDal, EfWishlistDal>();
builder.Services.AddScoped<IWishlistService, WishlistManager>();

//builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
//builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();


// FluentValidation yapýlandýrmasý
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>();
        fv.DisableDataAnnotationsValidation = true;
        fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
    });


// IHttpContextAccessor eklenmesi
builder.Services.AddHttpContextAccessor();


// SignalR
builder.Services.AddSignalR();

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("PaymentApiCors", opts =>
    {
        opts.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    });
});

// JWT yapýlandýrmasý
var jwtKey = builder.Configuration["JWT_KEY"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT_KEY is not configured.");
}

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Geliþtirme ortamý için
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtTokenDefaults.ValidIssuer,
        ValidAudience = JwtTokenDefaults.ValidAudience,
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero
    };
});

// Swagger/OpenAPI yapýlandýrmasý
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });

    // JWT için Swagger'a destek ekleme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                      "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
          },
          new List<string>()
        }
    });
});

// Uygulamanýn inþa edilmesi
var app = builder.Build();

// Middleware'lerin eklenmesi
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("PaymentApiCors");
app.UseAuthentication(); // Authentication ekleniyor
app.UseAuthorization();

// SignalR hub'larýnýn eklenmesi
app.MapHub<NotificationHub>("/hubs/notification");

// Controller'larýn eklenmesi
app.MapControllers();

app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
