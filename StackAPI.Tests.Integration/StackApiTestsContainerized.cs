using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

using StackAPI.Tests.Integration.Helpers;

namespace StackAPI.Tests.Integration;
public class StackApiTestsContainerized(DatabaseFixture fixture)
    : IClassFixture<DatabaseFixture>, IAsyncLifetime {

  [Fact]
  public async void StackApi_ReturnsResponseList_OnLoadTagsFromStack() {
    //Arrange
    var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(host => {
        host.UseSetting(
          "Database:ConnectionString",
          fixture.ConnectionString
        );
      });
    var client = factory.CreateClient();

    //Act
    var response = await client.GetAsync("/populateDb");

    //Assert
    response.Should().NotBeNull();
    response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
  }

  public Task DisposeAsync() {
    return Task.CompletedTask;
  }

  public Task InitializeAsync() {
    return Task.CompletedTask;
  }
}
