using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts.IPersonService;

using Entities;

using Exceptions;

using Microsoft.Extensions.Logging;

using RepositoryContracts;

using Serilog;

using Service_Country.Helpers;

using ServiceContracts_Country.IPersonService;

using ServiceContracts.DTO;

namespace ContactsManager.Core.Services.PersonService
{
 public class PersonsUpdaterService : IPersonsUpdaterService
 {
  //private field
  private readonly IPersonsRepository            _personsRepository;
  private readonly ILogger<PersonsGetterService> _logger;
  private readonly IDiagnosticContext            _diagnosticContext;

  //constructor
  public PersonsUpdaterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
  {
   _personsRepository = personsRepository;
   _logger = logger;
   _diagnosticContext = diagnosticContext;
  }


  public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
  {
   if (personUpdateRequest == null)
    throw new ArgumentNullException(nameof(personUpdateRequest));

   //validation
   ValidationHelper.ModelValidation(personUpdateRequest);

   //get matching person object to update
   Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
   if (matchingPerson == null)
   {
    throw new InvalidPersonIdException("Given person id doesn't exist");
   }

   //update all details
   matchingPerson.PersonName = personUpdateRequest.PersonName;
   matchingPerson.Email = personUpdateRequest.Email;
   matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
   matchingPerson.Gender = personUpdateRequest.Gender.ToString();
   matchingPerson.CountryID = personUpdateRequest.CountryID;
   matchingPerson.Address = personUpdateRequest.Address;
   matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

   await _personsRepository.UpdatePerson(matchingPerson); //UPDATE

   return matchingPerson.ToPersonResponse();
  }
 }
}
