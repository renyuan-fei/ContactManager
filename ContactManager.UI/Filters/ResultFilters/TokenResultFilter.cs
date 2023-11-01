using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.ResultFilters;

public class TokenResultFilter : IResultFilter
{
  public void OnResultExecuting(ResultExecutingContext context)
  {
    // 需要在请求开始前设置Cookie
    context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
  }

  public void OnResultExecuted(ResultExecutedContext context)
  {
  }
}
