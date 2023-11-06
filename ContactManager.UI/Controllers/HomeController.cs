using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers
{
  public class HomeController : Controller
  {
    [Route("Error")]
    public IActionResult Error()
    {
      // 获取异常信息
      IExceptionHandlerPathFeature ? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

      // 检查异常信息是否为空
      if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
      {
        ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
      }
      return View(); //Views/Shared/Error
    }

  }
}
