using Fizzler.Systems.HtmlAgilityPack;

using FluentAssertions;

using HtmlAgilityPack;

namespace ContactManager.IntegrationTests;

public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;

  public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
  {
    _client = factory.CreateClient();
  }

  #region Index
  [ Fact ]
  public async Task Index_ToReturnView()
  {
    //Arrange

    //Act
    var response = await _client.GetAsync("/Persons/Index");

    //Assert
    response.Should().BeSuccessful(); //2xx

    var responseBody = await response.Content.ReadAsStringAsync();

    var html = new HtmlDocument();
    html.LoadHtml(responseBody);
    var document = html.DocumentNode;

    document.QuerySelectorAll("table.persons").Should().NotBeNull();
  }
  #endregion
}