using ContactManager.UI.Filters.ActionFilters;
using ContactManager.UI.Middleware;
using ContactManager.UI.StartupExtensions;

using ContactsManager.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((
                            context,
                            services,
                            loggerConfiguration) =>
                        {
                          loggerConfiguration
                              // 从built-in configuration中读取配置
                              .ReadFrom.Configuration(context.Configuration)
                              // 读取当前app的服务，使其对于serilog可用
                              .ReadFrom.Services(services);
                        });

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(builder
                       .Configuration
                       .GetConnectionString("DefaultConnection"));
});

// 注册filter到DI容器中，之后由service provider提供实例
builder.Services.AddTransient<PersonsListActionFilter>();

builder.Services.AddHttpLogging(options =>
{
  options.LoggingFields =
      Microsoft.AspNetCore.HttpLogging.HttpLoggingFields
               .RequestProperties
    | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields
               .ResponsePropertiesAndHeaders;
});

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

var app = builder.Build();

if (builder.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
else
{
  app.UseExceptionHandler("/Error");
  app.UseExceptionHandlingMiddleware();
}

// if (builder.Environment.IsEnvironment("Test") == false)
// {
//   Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot",
//                                                   wkhtmltopdfRelativePath: "Rotativa");
// }
// 启动https
//-------------------------------------------------------------------------------
app.UseHsts();
app.UseHttpsRedirection();
//-------------------------------------------------------------------------------
app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseRouting(); // identifying action method based route

app.UseAuthentication(); // reading identity cookie
app.UseAuthorization(); // verifying identity
app.MapControllers(); // execute the filter pipeline (action + result filters)

// 定义路由
app.UseEndpoints(endpoints => {
  endpoints.MapControllerRoute(
                               name: "areas",
                               pattern: "{area:exists}/{controller=Home}/{action=Index}");

  //Admin/Home/Index
  //Admin

  endpoints.MapControllerRoute(
                               name: "default",
                               pattern: "{controller}/{action}/{id?}"
                              );
});

app.Run();

public partial class Program
{
} //make the auto-generated Program accessible programmatically
