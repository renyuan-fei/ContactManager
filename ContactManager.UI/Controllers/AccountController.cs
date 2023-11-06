using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers;

// 这是一种Route方式，此外还可以在Startup.cs中配置全局路由
// [ Route("[controller]/[action]") ]
// [ AllowAnonymous ]
public class AccountController : Controller
{
  // 用于管理用户的类
  // 提供了创建用户、删除用户、修改用户、查询用户等功能
  private readonly UserManager<ApplicationUser> _userManager;

  private readonly SignInManager<ApplicationUser> _signInManager;

  private readonly RoleManager<ApplicationRole> _roleManager;

  public AccountController(
      UserManager<ApplicationUser>   userManager,
      SignInManager<ApplicationUser> signInManager,
      RoleManager<ApplicationRole>      roleManager)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _roleManager = roleManager;
  }

  [ HttpGet ]
  // 自定义policy，用于检测用户是否已经登录，如果已经登录，返回true，否则返回false
  [Authorize("NotAuthorized")]
  public IActionResult Register() { return View(); }

  [ HttpPost ]
  [Authorize("NotAuthorized")]
  public async Task<IActionResult> Register(RegisterDTO registerDTO)
  {
    // 检查模型是否有效
    if (ModelState.IsValid == false)
    {
      ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors)
                                 .Select(x => x.ErrorMessage);

      return View(registerDTO);
    }

    // 创建用户
    ApplicationUser user = new ApplicationUser()
    {
        Email = registerDTO.Email,
        PhoneNumber = registerDTO.Phone,
        UserName = registerDTO.Email,
        PersonName = registerDTO.PersonName
    };

    // 将用户添加到数据库
    var result = await _userManager.CreateAsync(user, registerDTO.Password);

    // 检查是否成功
    if (result.Succeeded)
    {
      //check status of radio button
      if (registerDTO.UserType == UserTypeOptions.Admin)
      {
        // create 'Admin' role,这只是一个role，不是用户
        // 如果数据库中没有'Admin' role，创建一个该role
        if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
        {
          ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
          await _roleManager.CreateAsync(applicationRole);
        }
        // add 'Admin' role to user
        IdentityResult roleResult =
            await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
      }
      else
      {
        // 由于默认的role是'User'，所以不需要创建'User' role
        // add 'User' role to user
        IdentityResult roleResult = await _userManager.AddToRoleAsync(user,
          UserTypeOptions.User.ToString());
      }

      // 登录用户
      await _signInManager.SignInAsync(user, false);

      // 重定向到登录页面
      return RedirectToAction(nameof(PersonsController.Index), "Persons");
    }

    // 如果失败，将错误信息添加到模型中
    foreach (var identityError in result.Errors)
    {
      ModelState.AddModelError("", identityError.Description);
    }

    // 返回视图
    return View(registerDTO);
  }

  [ HttpGet ]
  [Authorize("NotAuthorized")]
  public IActionResult Login() { return View(); }

  [ HttpPost ]
  [Authorize("NotAuthorized")]
  public async Task<IActionResult> Login(LoginDTO loginDTO, string? returnUrl)
  {
    if (ModelState.IsValid == false)
    {
      ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors)
                                 .Select(x => x.ErrorMessage);

      return View(loginDTO);
    }

    var result = await _signInManager
        .PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

    if (result.Succeeded)
    {
      //admin
      ApplicationUser user = _userManager.FindByEmailAsync(loginDTO.Email).Result;


      if (user != null)
      {
        if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
        {
          return RedirectToAction("Index", "Home" , new { area = "Admin" });
        }
      }

      // 如果有returnUrl，重定向到returnUrl
      if (!string.IsNullOrEmpty(returnUrl)
       && Url.IsLocalUrl(returnUrl)) { return Redirect(returnUrl); }

      return RedirectToAction(nameof(PersonsController.Index), "Persons");
    }

    ModelState.AddModelError("Login", "Invalid login or password");

    return View(loginDTO);
  }

  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();

    return RedirectToAction(nameof(PersonsController.Index), "Persons");
  }

  // remote validation
  [ AllowAnonymous ]
  public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
  {
    ApplicationUser user = await _userManager.FindByEmailAsync(email);

    if (user == null)
    {
      return Json(true); //valid
    }
    else
    {
      return Json(false); //invalid
    }
  }
}
