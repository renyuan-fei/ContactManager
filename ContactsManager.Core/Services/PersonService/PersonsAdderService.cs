﻿using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts.IPersonService;

using Entities;

using Microsoft.Extensions.Logging;

using RepositoryContracts;

using Serilog;

using Service_Country.Helpers;

using ServiceContracts_Country.IPersonService;

using ServiceContracts.DTO;

namespace ContactsManager.Core.Services.PersonService
{
 public class PersonsAdderService : IPersonsAdderService
 {
  //private field
  private readonly IPersonsRepository            _personsRepository;
  private readonly ILogger<PersonsGetterService> _logger;
  private readonly IDiagnosticContext            _diagnosticContext;

  //constructor
  public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
  {
   _personsRepository = personsRepository;
   _logger = logger;
   _diagnosticContext = diagnosticContext;
  }


  public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
  {
   //check if PersonAddRequest is not null
   if (personAddRequest == null)
   {
    throw new ArgumentNullException(nameof(personAddRequest));
   }

   //Model validation
   ValidationHelper.ModelValidation(personAddRequest);

   //convert personAddRequest into Person type
   Person person = personAddRequest.ToPerson();

   //generate PersonID
   person.PersonID = Guid.NewGuid();

   //add person object to persons list
   await _personsRepository.AddPerson(person);

   //convert the Person object into PersonResponse type
   return person.ToPersonResponse();
  }
 }
}
