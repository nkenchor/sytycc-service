using System.Net;
using System.Text.Json;

namespace Sytycc_Service.Domain;

public class AppExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public AppExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
           
            await _next(context);
            
           
        }
        catch (Exception error)
        {
            var response = context.Response;
            var request = context.Request;
            response.ContentType = "application/json";
            AppExceptionResponse exceptionResponse = null;
            switch (error)
            {
                case AppException e:
                    response.StatusCode = e.StatusCode;
                    exceptionResponse = new AppExceptionResponse(e);
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(exceptionResponse);
            await response.WriteAsync(result);
        }
    }
}