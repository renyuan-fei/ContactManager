using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.AuthorizationFilters;

public class TokenAuthorizationFilter : IAuthorizationFilter, IOrderedFilter
{
  private readonly ILogger<TokenAuthorizationFilter> _logger;
  public int Order { get; }

  public TokenAuthorizationFilter(int order, ILogger<TokenAuthorizationFilter> logger)
  {
    Order = order;
    _logger = logger;
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    _logger.LogInformation("{FilterName}.{MethodName} - before action method",
                           nameof(TokenAuthorizationFilter),
                           nameof(OnAuthorization));

    //TO DO: authorization logic
    if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
    {
      context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }

    if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
    {
      context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }

    _logger.LogInformation("{FilterName}.{MethodName} - after action method",
                           nameof(TokenAuthorizationFilter),
                           nameof(OnAuthorization));
  }
}
