using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.UI.Filters.ActionFilters;

public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
{
  public  bool    IsReusable => false;
  private string? Key        { get; set; }
  private string? Value      { get; set; }
  private int     Order      { get; set; }

  public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
  {
    Key = key;
    Value = value;
    Order = order;
  }

  //Controller -> FilterFactory -> Filter
  public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
  {
    // 从serviceProvider中获取需要的service
    var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
    filter.Key = Key;
    filter.Value = Value;
    filter.Order = Order;

    //return filter object
    return filter;
  }
}

// interface
public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
{
  // 用于存储通过filter参数传递的值
  public           int                                 Order { get; set; }
  public           string?                             Key { get; set; }
  public           string?                             Value { get; set; }
  private readonly ILogger<ResponseHeaderActionFilter> _logger;

  // 其他参数通过FilterFactory传递
  public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
  {
    _logger = logger;
  }

  // 通过构造函数传递参数
  // public ResponseHeaderActionFilter(
  //     ILogger<ResponseHeaderActionFilter> logger,
  //     string?                             key,
  //     string?                             value,
  //     int                                 order)
  // {
  //   _logger = logger;
  //
  //   // 通过filter参数传递的值
  //   Key = key;
  //   Value = value;
  //   Order = order;
  // }

  // public void OnActionExecuting(ActionExecutingContext context)
  // {
  //   _logger.LogInformation("{FilterName}.{MethodName} method",
  //                          nameof(ResponseHeaderActionFilter),
  //                          nameof(OnActionExecuting));
  // }
  //
  // public void OnActionExecuted(ActionExecutedContext context)
  // {
  //   _logger.LogInformation("{FilterName}.{MethodName} method",
  //                          nameof(ResponseHeaderActionFilter),
  //                          nameof(OnActionExecuting));
  //
  //   // 将key和value作为键值对放入Header中(仅用于演示filter的值传递)
  //   context.HttpContext.Response.Headers[_key] = _value;
  // }

  public async Task OnActionExecutionAsync(
      ActionExecutingContext  context,
      ActionExecutionDelegate next)
  {
    // before executing the action
    _logger.LogInformation("{FilterName}.{MethodName} method",
                           nameof(ResponseHeaderActionFilter),
                           nameof(OnActionExecutionAsync));

    await next();

    // after executing the action
    _logger.LogInformation("{FilterName}.{MethodName} method",
                           nameof(ResponseHeaderActionFilter),
                           nameof(OnActionExecutionAsync));

    // 将key和value作为键值对放入Header中(仅用于演示filter的值传递)
    if (Key != null) context.HttpContext.Response.Headers[Key] = Value;
  }
}

// // attribute
// public class ResponseHeaderActionFilter : ActionFilterAttribute
// {
//   private readonly string _key;
//   private readonly string _value;
//
//   public ResponseHeaderActionFilter(string key, string value, int order)
//   {
//     _key = key;
//     _value = value;
//     Order = order;
//   }
//
//   public async override Task OnActionExecutionAsync(
//       ActionExecutingContext  context,
//       ActionExecutionDelegate next)
//   {
//     await next(); //calls the subsequent filter or action method
//
//     context.HttpContext.Response.Headers[_key] = _value;
//   }
// }
