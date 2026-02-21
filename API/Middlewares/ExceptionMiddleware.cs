using System.Net;
using System.Text.Json;
using API.Errors;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace API.Middlewares;


public class ExceptionMiddleware(IHostingEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, IHostingEnvironment hostingEnvironment)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = hostingEnvironment.IsDevelopment() 
            ? new ApiErrorResponse(context.Response.StatusCode, exception.Message, exception.StackTrace)
            : new ApiErrorResponse(context.Response.StatusCode, exception.Message, "Internal server error");
        
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var json = JsonSerializer.Serialize(response, options);
        
        return context.Response.WriteAsync(json);
    }
}