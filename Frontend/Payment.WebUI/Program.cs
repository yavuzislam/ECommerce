using FluentValidation.AspNetCore;
using Payment.WebUI.ValidationRules.RegisterValidationRules;
using System.Globalization;
using Payment.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Payment.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Payment.WebUI.Services.AuthServices;
using Payment.WebUI.Services.CategoryServices;
using Payment.WebUI.Services.TokenServices;
using Payment.WebUI.Services.ProductServices;
using Payment.WebUI.Services.MailServices;
using Payment.WebUI.Services.UserServices;
using Payment.WebUI.Services.RoleServices;
using Payment.WebUI.Services.UserRoleServices;
using Payment.WebUI.Services.AddressServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

string connectionString = builder.Configuration.GetConnectionString("MsSqlConnection");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>(
    i => new EmailSenderService(
        builder.Configuration["EmailSender:Host"],
        builder.Configuration.GetValue<int>("EmailSender:Port"),
        builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
        builder.Configuration["EmailSender:Username"],
        builder.Configuration["EmailSender:Password"])
);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/ErrorPage";
    options.SlidingExpiration = true;
});



builder.Services.AddAuthentication();


builder.Services.AddControllersWithViews().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>();
    fv.DisableDataAnnotationsValidation = true;
    fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
