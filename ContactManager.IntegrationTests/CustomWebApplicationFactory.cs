// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;

using ContactsManager.Infrastructure.DbContext;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManager.IntegrationTests;

// 这里的 Program 是指 Web 项目的 Program 类
// 如果出现using Microsoft.VisualStudio. TestPlatform.TestHost;请注意是否引用了错误的包
// 为了让program可以被引用，需要在web项目中添加一个空的program类，见 program.cs 最后一行
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    base.ConfigureWebHost(builder);

    // 使用 Test 环境
    builder.UseEnvironment("Test");

    // 使用内存数据库
    builder.ConfigureServices(services =>
    {
      // 移除原有的 DbContextOptions<ApplicationDbContext>
      var descripter =
          services.SingleOrDefault(temp => temp.ServiceType
                                        == typeof(
                                               DbContextOptions<ApplicationDbContext>));

      if (descripter != null) { services.Remove(descripter); }

      // 添加新的 DbContextOptions<ApplicationDbContext>，用于测试
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options
            .UseInMemoryDatabase("DatbaseForTesting");
      });
    });
  }
}
