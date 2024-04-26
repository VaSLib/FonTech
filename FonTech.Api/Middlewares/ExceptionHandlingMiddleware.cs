using FonTech.Domain.Enum;
using FonTech.Domain.Result;
using ILogger = Serilog.ILogger;

namespace FonTech.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync (HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExeptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExeptionAsync(HttpContext httpContext, Exception exeption)
    {
        _logger.Error(exeption, exeption.Message);

        var errorMessage = exeption.Message;
        var response = exeption switch
        {
            UnauthorizedAccessException ex => new BaseResult()
            {
                ErrorMessage = ex.Message,
                ErrorCode = (int)ErrorCodes.UserUnauthorizedAccess
            },
            _ => new BaseResult() 
            { 
                ErrorMessage = "Internal server error. Please retry later" ,
                ErrorCode = (int)ErrorCodes.InternalServerError
            },
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)response.ErrorCode;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
