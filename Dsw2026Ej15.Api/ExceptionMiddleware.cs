using System.Net;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Api;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { error = "Ocurrió un error inesperado.", detail = ex.Message });
        }
    }
}
