using Serilog;
using System.Net;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Beklenmeyen bir hata oluştu.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        if (exception is ArgumentNullException)
        {
            code = HttpStatusCode.BadRequest;
        }
        else if (exception is UnauthorizedAccessException)
        {
            code = HttpStatusCode.Unauthorized;
        }
        else if (exception is KeyNotFoundException)
        {
            code = HttpStatusCode.NotFound;
        }

        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
        {
            error = exception.Message,

        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}