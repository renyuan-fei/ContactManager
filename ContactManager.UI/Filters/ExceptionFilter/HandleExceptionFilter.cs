using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.ExceptionFilter;

public class HandleExceptionFilter : IExceptionFilter
{
  private readonly ILogger<HandleExceptionFilter> _logger;
  private readonly IHostEnvironment               _hostEnvironment;


  public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
  {
    _logger = logger;
    _hostEnvironment = hostEnvironment;
  }

  public void OnException(ExceptionContext context)
  {
    _logger.LogError("{FilterName}.{MethodName}\n{ExceptionType} - {ExceptionMessage}",
                     nameof(HandleExceptionFilter),
                     nameof(OnException),
                     context.Exception.GetType().Name,
                     context.Exception.Message);

    // 指定开发环境下才返回异常信息
    if (_hostEnvironment.IsDevelopment())
      context.Result =
          new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
  }

}
