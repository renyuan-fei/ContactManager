using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
///   Person domain model class
/// </summary>
public class Person
{
  [ Key ]
  public Guid PersonID { get; set; }

  [ StringLength(40) ]
  public string? PersonName { get; set; }

  [ StringLength(40) ]
  public string? Email { get; set; }

  public DateTime? DateOfBirth { get; set; }

  [ StringLength(10) ]
  public string? Gender { get; set; }

  [ StringLength(200) ]
  public string? Address { get; set; }

  public bool ReceiveNewsLetters { get; set; }

  public string? TIN { get; set; }

  public Guid? CountryID { get; set; }

  // 用于多对一关系，不会映射到数据库,指定外键
  // [ForeignKey("CountryID")]
  public Country? Country { get; set; } = null!;

  // unique identifier
  public override string ToString()
  {
    return
        $"{nameof(PersonID)}: {PersonID}, {nameof(PersonName)}: {PersonName}, {nameof(Email)}: {Email}, {nameof(DateOfBirth)}: {DateOfBirth}, {nameof(Gender)}: {Gender}, {nameof(Address)}: {Address}, {nameof(ReceiveNewsLetters)}: {ReceiveNewsLetters}, {nameof(TIN)}: {TIN}, {nameof(CountryID)}: {CountryID}, {nameof(Country)}: {Country}";
  }
}
