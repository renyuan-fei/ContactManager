using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.ResultFilters;

public class PersonsListResultFilter : IAsyncResultFilter, IOrderedFilter
{
  readonly ILogger<PersonsListResultFilter> _logger;
  public   int                              Order { get; }

  public PersonsListResultFilter(int order, ILogger<PersonsListResultFilter> logger)
  {
    Order = order;
    _logger = logger;
  }

  public async Task OnResultExecutionAsync(
      ResultExecutingContext  context,
      ResultExecutionDelegate next)
  {
    _logger.LogInformation("{FilterName}.{MethodName} - before action method",
                           nameof(PersonsListResultFilter),
                           nameof(OnResultExecutionAsync));

    await next();

    _logger.LogInformation("{FilterName}.{MethodName} - after action method",
                           nameof(PersonsListResultFilter),
                           nameof(OnResultExecutionAsync));

    context.HttpContext.Response.Headers.Add("XLast-Modified",
                                             DateTime.UtcNow
                                                     .ToString("yyyy-MM-dd HH:mm:ss"));
  }
}
