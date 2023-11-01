using ContactManager.UI.Filters.ActionFilters;

using ContactsManager.Core.ServiceContracts.ICountriesService;
using ContactsManager.Core.Services.CountriesService;
using ContactsManager.Core.Services.PersonService;
using ContactsManager.Infrastructure.DbContext;
using ContactsManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

using RepositoryContracts;

using ServiceContracts_Country.ICountriesService;
using ServiceContracts_Country.IPersonService;

namespace ContactManager.UI.StartupExtensions
{

  public static class ConfigureServicesExtension
  {
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
      services.AddControllersWithViews(options =>
      {
        // interface filter
        // 从services中获取logger的实例
        // var logger = builder.Services.BuildServiceProvider()
        //                     .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
        //
        // // 将filter添加到全局filter集合中，并传递logger实例
        // options.Filters.Add(new ResponseHeaderActionFilter(logger,
        //                                                    "Key-From-Global",
        //                                                    "value-From-Global",
        //                                                    0));

        // Filter Factory
        var logger = services.BuildServiceProvider()
                             .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

        options.Filters.Add(new ResponseHeaderActionFilter(logger)
        {
            Key = "My-Key-From-Global",
            Value = "My-Value-From-Global",
            Order = 2
        });

        // attribute filter
        // options.Filters.Add(new ResponseHeaderActionFilter(
        //                                                    "Key-From-Global",
        //                                                    "value-From-Global",
        //                                                    0));
      });

      //add services into IoC container

      services.AddScoped<ICountriesRepository, CountriesRepository>();
      services.AddScoped<IPersonsRepository, PersonsRepository>();

      services.AddScoped<ICountriesGetterService, CountriesGetterService>();
      services.AddScoped<ICountriesAdderService, CountriesAdderService>();
      services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();

      // 1
      // // 重写IPersonsGetterService接口的实现，作为依赖注入使用
      // services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
      // // 原本的实现注入到重写的实现中
      // services.AddScoped<PersonsGetterService, PersonsGetterService>();

      // 2
      // 继承PersonsGetterService类，，并只重写需要重写的方法(需要将其父类中的方法设置为virtual)
      services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild>();

      services.AddScoped<IPersonsAdderService, PersonsAdderService>();
      services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
      services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
      services.AddScoped<IPersonsSorterService, PersonsSorterService>();

      services.AddScoped<ResponseHeaderActionFilter>();

      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(configuration
                                 .GetConnectionString("DefaultConnection"));
      });

      // 注册filter到DI容器中，之后由service provider提供实例
      services.AddTransient<PersonsListActionFilter>();

      services.AddHttpLogging(options =>
      {
        options.LoggingFields =
            Microsoft.AspNetCore.HttpLogging.HttpLoggingFields
                     .RequestProperties
          | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields
                     .ResponsePropertiesAndHeaders;
      });

      return services;
    }
  }

}
