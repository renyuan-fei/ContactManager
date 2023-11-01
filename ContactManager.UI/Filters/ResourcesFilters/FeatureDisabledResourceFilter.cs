using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.ResourcesFilters;

public class FeatureDisabledResourceFilter : IAsyncResourceFilter, IOrderedFilter
{
  private readonly ILogger<FeatureDisabledResourceFilter> _logger;

  private readonly bool _isDisabled;
  public FeatureDisabledResourceFilter(int order,
      ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
  {
    Order = order;
    _logger = logger;
    _isDisabled = isDisabled;
  }

  public int Order { get; }

  public async Task OnResourceExecutionAsync(
      ResourceExecutingContext  context,
      ResourceExecutionDelegate next)
  {
    _logger.LogInformation("{FilterName}.{MethodName} - before action method",
                           nameof(FeatureDisabledResourceFilter),
                           nameof(OnResourceExecutionAsync));

    if (_isDisabled)
    {
      // context.Result = new StatusCodeResult(501); //501 - Not Implemented
      context.Result = new NotFoundResult(); //404 - Not Found
    }

    await next();


    _logger.LogInformation("{FilterName}.{MethodName} - after action method",
                           nameof(FeatureDisabledResourceFilter),
                           nameof(OnResourceExecutionAsync));
  }
}
