using ContactsManager.Core.DTO;

using ServiceContracts_Country.IPersonService;

namespace ContactsManager.Core.Services.PersonService;

public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
{
  // 继承IPersonsGetterService接口

  // 将原本的PersonsGetterService类作为私有字段，进行注入
  private readonly PersonsGetterService _personsGetterService;

  // 依赖注入
  public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
  {
    _personsGetterService = personsGetterService;
  }

  // 不需要重写的方法，直接调用原本的PersonsGetterService类中的方法
  public Task<List<PersonResponse>> GetAllPersons()
  {
    return _personsGetterService.GetAllPersons();
  }

  public Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
  {
    return _personsGetterService.GetPersonByPersonID(personID);
  }

  public Task<List<PersonResponse>> GetFilteredPersons(
      string  searchBy,
      string? searchString)
  {
    return _personsGetterService.GetFilteredPersons(searchBy, searchString);
  }

  public Task<MemoryStream> GetPersonsCSV()
  {
    return _personsGetterService.GetPersonsCSV();
  }

  public Task<MemoryStream> GetPersonsExcel()
  {
    //重写GetPersonsExcel方法
    //这里只是作为演示，需要在实际项目中根据需求进行重写
    return _personsGetterService.GetPersonsExcel();
  }
}
