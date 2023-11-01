using System.Net;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.ResultFilters;

public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter, IOrderedFilter
{
  private readonly ILogger<PersonAlwaysRunResultFilter> _logger;
  public           int                                  Order { get; }

  public PersonAlwaysRunResultFilter(
      ILogger<PersonAlwaysRunResultFilter> logger,
      int                                  order)
  {
    _logger = logger;
    Order = order;
  }

  public void OnResultExecuting(ResultExecutingContext context)
  {
    //skip the filter
    // 如果有 SkipFilter 则跳过
    if (context.Filters.OfType<SkipFilter>().Any()) { }
  }

  public void OnResultExecuted(ResultExecutedContext context)
  {
    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
  }
}
