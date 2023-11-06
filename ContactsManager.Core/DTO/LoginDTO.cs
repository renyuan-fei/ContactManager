using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO;

public class LoginDTO
{
  [Required(ErrorMessage = "Email is required")]
  [EmailAddress(ErrorMessage = "Email is not valid")]
  public string? Email { get; set; }

  [Required(ErrorMessage = "Password is required")]
  [DataType(DataType.Password)]
  public string? Password { get; set; }
}
