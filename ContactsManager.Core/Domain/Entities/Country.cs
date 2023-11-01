using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
///   Domain Model for Country
/// </summary>
public class Country
{
  [ Key ]
  public Guid CountryID { get; set; }

  public string? CountryName { get; set; }

  // 用于一对多关系，不会映射到数据库
  public ICollection<Person>? Persons { get; set; } = new List<Person>();
}
