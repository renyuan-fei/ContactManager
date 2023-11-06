using ContactsManager.Core.DTO;

using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContracts.IPersonService;

//ISP
// ISP - Interface Segregation Principle
// 将接口根据不同的功能进行拆分，避免接口过于臃肿，不同的功能应该由不同的接口来实现
// 此处将CRUD拆分为不同的接口

/// <summary>
///   Represents business logic for manipulating Perosn entity
/// </summary>
public interface IPersonsAdderService
{
  /// <summary>
  ///   Addds a new person into the list of persons
  /// </summary>
  /// <param name="personAddRequest">Person to add</param>
  /// <returns>Returns the same person details, along with newly generated PersonID</returns>
  Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
}
