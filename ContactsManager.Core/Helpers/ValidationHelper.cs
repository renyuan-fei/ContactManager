using System.ComponentModel.DataAnnotations;

namespace Service_Country.Helpers;

public class ValidationHelper
{
  internal static void ModelValidation(object obj)
  {
    //Model validations
    var validationContext = new ValidationContext(obj);

    var validationResults =
        new List<ValidationResult>();

    var isValid = Validator.TryValidateObject(obj,
                                              validationContext,
                                              validationResults,
                                              true);

    if (!isValid)
    {
      throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
    }
  }
}
