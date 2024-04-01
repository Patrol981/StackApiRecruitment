using System.Net.Http.Json;

using Bogus;

using FluentAssertions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;
using StackAPI.Models.Mapping;
using StackAPI.Models.Responses;
using StackAPI.Repositories;
using StackAPI.Services;
using StackAPI.Tests.Integration.Helpers;

namespace StackAPI.Tests.Integration;
public class TagTestContainerized(DatabaseFixture fixture)
    : IClassFixture<DatabaseFixture>, IAsyncLifetime {

  [Fact]
  public async void Tag_ReturnsTagResponseList_OnGetTagRequestWithoutParam() {
    //Arrange
    var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(host => {
        host.UseSetting(
          "Database:ConnectionString",
          fixture.ConnectionString
        );
        host.UseEnvironment(Environments.Staging);
      });
    var client = factory.CreateClient();
    var tagService = factory.Services.GetRequiredService<ITagService>();

    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From($"tag_{f.IndexGlobal}"))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    await tagService.CreateManyAsync(tags);

    //Act
    var response = await client.GetAsync("/tags");

    //Assert
    response.Should().NotBeNull();
    response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);

    var result = await response.Content.ReadFromJsonAsync<GetAllTagsResponse>();
    result.Should().NotBeNull();
    result!.Tags.Should().NotBeNullOrEmpty();
    result.TotalPages.Should().NotBeNull();
    result.TotalRecords.Should().NotBeNull();
    result.CurrentPage.Should().NotBeNull();
    result.PageSize.Should().NotBeNull();

    result.Tags.Should().HaveCountGreaterThan(0);
    result.Tags.Should().HaveCount((int)result.PageSize!);
    result.TotalPages.Should().BeInRange(
      (tags.Count() / result.PageSize!.Value) - 1,
      (tags.Count() / result.PageSize!.Value) + 1
    );
    result.CurrentPage.Should().Be(0);
    result.TotalRecords.Should().Be(tags.Count());
    result.PageSize.Should().Be(100);
  }

  [Fact]
  public async void Tag_ReturnsCorrectNumber_OnGetRecordNumber() {
    // Arrange
    var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(host => {
        host.UseSetting(
          "Database:ConnectionString",
          fixture.ConnectionString
        );
        host.UseEnvironment(Environments.Staging);
      });
    var client = factory.CreateClient();
    var tagService = factory.Services.GetRequiredService<ITagService>();

    var exampleTags = TagNameGenerator.GetTagNames(2000);
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    await tagService.CreateManyAsync(tags);
    var allTags = await tagService.GetAllAsync();

    // Act
    var result = await tagService.GetTotalTagsNumber();

    // Assert
    result.TotalCount.Should().Be(allTags.Count());
  }

  [Fact]
  public async void Tag_TagsLengthMatch_OnAddingManyTagsWithSameName() {
    // Arrange
    var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(host => {
        host.UseSetting(
          "Database:ConnectionString",
          fixture.ConnectionString
        );
        host.UseEnvironment(Environments.Staging);
      });
    var client = factory.CreateClient();
    var tagService = factory.Services.GetRequiredService<ITagService>();

    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();

    // Act
    await tagService.CreateManyAsync(tags);
    var result = await tagService.GetAllAsync();

    // Assert
    result.Count().Should().Be(exampleTags.Length);
  }

  [Fact]
  public async void Tag_TagsBeingDeletedProperly_OnDeletgingManyTags() {
    // Arrange
    var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(host => {
        host.UseSetting(
          "Database:ConnectionString",
          fixture.ConnectionString
        );
        host.UseEnvironment(Environments.Staging);
      });
    var client = factory.CreateClient();
    var tagService = factory.Services.GetRequiredService<ITagService>();

    var exampleTags = TagNameGenerator.GetTagNames(2000);
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    await tagService.CreateManyAsync(tags);
    var tagsToDelete = tags.ToDeleteTagRequests();

    // Act
    var deleteResposne = await client.PostAsJsonAsync("/tags/removeMany", tagsToDelete);
    var getAllResponse = await client.GetAsync("/tags");

    // Assert
    deleteResposne.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status204NoContent);

    var getAllResult = await getAllResponse.Content.ReadFromJsonAsync<GetAllTagsResponse>();
    getAllResult.Should().NotBeNull();
    getAllResult!.Tags.Should().NotBeEquivalentTo(tags);
    getAllResult.TotalRecords.Should().Be(0);
    getAllResult.TotalPages.Should().Be(0);
    getAllResult.PageSize.Should().Be(100);
    getAllResult.CurrentPage.Should().Be(0);
  }

  [Fact]
  public async void Tag_TagShareShouldBeAppropriate_OnGetTagShare() {
    // Arrange
    var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(host => {
        host.UseSetting(
          "Database:ConnectionString",
          fixture.ConnectionString
        );
        host.UseEnvironment(Environments.Staging);
      });
    var client = factory.CreateClient();
    var tagService = factory.Services.GetRequiredService<ITagService>();
    var tagRepository = factory.Services.GetRequiredService<ITagRepository>();

    var exampleTags = new[] { "csharp", "cpp", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(100, 5000)));
    var tags = tagsFaker.GenerateBetween(5, 50).AsEnumerable();
    await tagService.CreateManyAsync(tags);
    var targetTag = tags.ToArray()[0];
    var totalCount = await tagRepository.GetTotalCountAsync();

    // Act
    var response = await client.GetAsync($"/tags/share/{targetTag.Name}");
    var result = await response.Content.ReadFromJsonAsync<GetTagShareResponse>();

    // Assert
    response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);

    result.Should().NotBeNull();

    double expectedPercentage = (targetTag.Count.Value / (double)totalCount.TotalCount) * 100;
    result!.TagSharePercentage.Should().Be((decimal)expectedPercentage);
  }

  public Task DisposeAsync() {
    return Task.CompletedTask;
  }

  public Task InitializeAsync() {
    return Task.CompletedTask;
  }
}
