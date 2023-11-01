using Entities;

namespace RepositoryContracts;

/// <summary>
///   Represents data access logic for managing Country entity
/// </summary>
public interface ICountriesRepository
{
  /// <summary>
  ///   Add Country to the list of countries
  /// </summary>
  /// <param name="country">Country object to add</param>
  /// <returns>Returns the Country object after adding it to the table</returns>
  Task<Country> AddCountry(Country country);

  /// <summary>
  ///   Return all countries in the data store
  /// </summary>
  /// <returns>List of country objects from table</returns>
  Task<List<Country>> GetAllCountries();

  /// <summary>
  ///   Return all countries in the data store
  /// </summary>
  /// <param name="countryId">CountryID to search</param>
  /// <returns>Matching country or null</returns>
  Task<Country?> GetCountryByCountryID(Guid countryId);

  /// <summary>
  ///   Return all countries in the data store
  /// </summary>
  /// <param name="countryName">Country name to search</param>
  /// <returns>Matching country or null</returns>
  Task<Country?> GetCountryByCountryName(string countryName);
}
