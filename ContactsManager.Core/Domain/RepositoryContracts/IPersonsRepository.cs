using System.Linq.Expressions;

using Entities;

namespace RepositoryContracts;

/// <summary>
///   Represents data access logic for managing Person entity
/// </summary>
public interface IPersonsRepository
{
  /// <summary>
  ///   Adds a person object to the data store
  /// </summary>
  /// <param name="person">Person object to add</param>
  /// <returns>Return the Person object after adding it to the Table</returns>
  Task<Person> AddPerson(Person person);

  /// <summary>
  ///   Return all persons in the data store
  /// </summary>
  /// <returns>return all person from table</returns>
  Task<List<Person>> GetAllPersons();

  /// <summary>
  ///   Get persons by person id
  /// </summary>
  /// <param name="personid">PersonID(Guid) to search</param>
  /// <returns>A person object or null</returns>
  Task<Person?> GetPersonByPersonID(Guid personid);

  /// <summary>
  ///   According to specific conditions, get the list of persons
  /// </summary>
  /// <param name="predicate">LING expressions to check(input a Person object,return bool)</param>
  /// <returns>ALl matching person</returns>
  Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

  /// <summary>
  ///   Deletes a person object based on the person id
  /// </summary>
  /// <param name="personid">PersonID(Guid) to search</param>
  /// <returns>
  ///   Returns true, if the deletion is successful; otherwise false
  /// </returns>
  Task<bool> DeletePersonByPersonID(Guid personid);

  /// <summary>
  ///   Updates a person object (person name and other details) based on the given person id
  /// </summary>
  /// <param name="person">Person object to update</param>
  /// <returns>Returns the updated person object</returns>
  Task<Person> UpdatePerson(Person person);
}
