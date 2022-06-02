using Application.Common.Exceptions;

namespace Api.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = GetStatusCode(e);
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(e.Message);
        }
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
}