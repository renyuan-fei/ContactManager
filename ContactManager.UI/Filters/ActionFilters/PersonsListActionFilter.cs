using ContactManager.UI.Controllers;

using ContactsManager.Core.DTO;

using Microsoft.AspNetCore.Mvc.Filters;

using ServiceContracts_Country.Enums;

using ServiceContracts.DTO;

namespace ContactManager.UI.Filters.ActionFilters;

public class PersonsListActionFilter : IActionFilter
{
  private readonly ILogger<PersonsListActionFilter> _logger;

  public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
  {
    _logger = logger;
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
    // context.HttpContext.Response.Headers.Add("X-Action-Filter", "PersonsListActionFilter");

    //To do: add before logic here
    _logger.LogInformation("{FilterName}.{Method} method",
                           nameof(PersonsListActionFilter),
                           nameof(OnActionExecuting));

    if (!context.ActionArguments.ContainsKey("searchBy")) return;

    var searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

    //validate the searchBy parameter value
    if (string.IsNullOrEmpty(searchBy)) return;

    var searchByOptions = new List<string>
    {
        nameof(PersonResponse.PersonName),
        nameof(PersonResponse.Email),
        nameof(PersonResponse.DateOfBirth),
        nameof(PersonResponse.Gender),
        nameof(PersonResponse.CountryID),
        nameof(PersonResponse.Address)
    };

    //reset the searchBy paramer value
    // if the searchBy value is not in the list of searchByOptions
    if (searchByOptions.Any(temp => temp == searchBy)) return;

    _logger.LogInformation("searchBy actual value {searchBy}", searchBy);

    // default option
    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
    _logger.LogInformation("searchBy updated value {searchBy}", searchBy);
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    // context.HttpContext.Response.Headers.Add("X-Action-Filter", "PersonsListActionFilter");

    //To do: add after logic here
    _logger.LogInformation("{FilterName}.{Method} method",
                           nameof(PersonsListActionFilter),
                           nameof(OnActionExecuted));

    var personsController = (PersonsController)context.Controller;

    var parameters =
        (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

    if (parameters != null)
    {
      if (parameters.ContainsKey("searchBy"))
      {
        personsController.ViewData["CurrentSearchBy"] =
            Convert.ToString(parameters["searchBy"]);
      }

      if (parameters.ContainsKey("searchString"))
      {
        personsController.ViewData["CurrentSearchString"] =
            Convert.ToString(parameters["searchString"]);
      }

      if (parameters.ContainsKey("sortBy"))
      {
        personsController.ViewData["CurrentSortBy"] =
            Convert.ToString(parameters["sortBy"]);
      }
      else
      {
        personsController.ViewData["CurrentSortBy"] = nameof(PersonResponse.PersonName);
      }

      if (parameters.ContainsKey("sortOrder"))
      {
        personsController.ViewData["CurrentSortOrder"] =
            Convert.ToString(parameters["sortOrder"]);
      }
      else
      {
        personsController.ViewData["CurrentSortOrder"] = nameof(SortOrderOptions.ASC);
      }
    }

    personsController.ViewBag.SearchFields = new Dictionary<string, string>
    {
        { nameof(PersonResponse.PersonName), "Person Name" },
        { nameof(PersonResponse.Email), "Email" },
        { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
        { nameof(PersonResponse.Gender), "Gender" },
        { nameof(PersonResponse.CountryID), "Country" },
        { nameof(PersonResponse.Address), "Address" }
    };
  }
}
