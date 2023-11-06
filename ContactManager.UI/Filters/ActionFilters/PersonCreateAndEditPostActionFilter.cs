using ContactManager.UI.Controllers;

using ContactsManager.Core.ServiceContracts.ICountriesService;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactManager.UI.Filters.ActionFilters;

public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
{
  private readonly ICountriesGetterService _countriesGetterService;

  public PersonCreateAndEditPostActionFilter(ICountriesGetterService countriesService)
  {
    _countriesGetterService = countriesService;
  }

  public async Task OnActionExecutionAsync(
      ActionExecutingContext  context,
      ActionExecutionDelegate next)
  {
    //TO DO: before logic

    if (context.Controller is PersonsController personsController)
    {
      if (!personsController.ModelState.IsValid)
      {
        var countries = await _countriesGetterService.GetAllCountries();

        personsController.ViewBag.Countries = countries.Select(temp =>
            new SelectListItem
            {
                Text = temp.CountryName, Value = temp.CountryID.ToString()
            });

        personsController.ViewBag.Errors = personsController.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var personRequest = context.ActionArguments["personRequest"];

        context.Result =
            personsController
                // ReSharper disable once Mvc.ViewNotResolved
                .View(personRequest); //short-circuits or skips the subsequent action filters & action method
      }
      else
      {
        await next(); //invokes the subsequent filter or action method
      }
    }
    else
    {
      await next(); //calls the subsequent filter or action method
    }

    //TO DO: before logic
  }
}
