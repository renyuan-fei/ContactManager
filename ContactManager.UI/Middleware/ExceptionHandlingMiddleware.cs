using System.Net;

using Serilog;

namespace ContactManager.UI.Middleware;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;

  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  private readonly IDiagnosticContext _diagnosticContext;

  public ExceptionHandlingMiddleware(
      RequestDelegate                      next,
      ILogger<ExceptionHandlingMiddleware> logger,
      IDiagnosticContext                   diagnosticContext)
  {
    _next = next;
    _logger = logger;
    _diagnosticContext = diagnosticContext;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // before logic

    // Call the next delegate/middleware in the pipeline.
    try
    {
      await _next(context);
    }
    catch (Exception e)
    {
      // 用于记录innerException
      if (e.InnerException != null)
      {
        _logger.LogError("{ExceptionType} : {Message}",
                         e.InnerException.GetType().ToString(),
                         e.InnerException.Message);

      }
      // 用于记录其他exception
      else
      {
        _logger.LogError("{ExceptionType} : {Message}",
                         e.GetType().ToString(),
                         e.Message);
      }

      // 用于返回错误信息
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      await context.Response.WriteAsync("Error occurred");
    }

    // after logic
  }
}

public static class ExceptionHandlingMiddlewareExtensions
{
  public static IApplicationBuilder UseExceptionHandlingMiddleware(
      this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ExceptionHandlingMiddleware>();
  }
}
