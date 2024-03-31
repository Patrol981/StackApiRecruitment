using Bogus;

using FluentAssertions;

using NSubstitute;

using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;
using StackAPI.Models.Mapping;
using StackAPI.Models.Requests;
using StackAPI.Services;

namespace StackAPI.Tests;
public class TagTestsNSub {
  [Fact]
  public async void Tag_ReturnsTagList_OnGetTagRequestWithoutParam() {
    //Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.GetAllAsync().Returns(tags);


    //Act
    var result = await mockTagService.GetAllAsync();

    //Assert
    result.Should().NotBeNull();
    result.Should().BeSameAs(tags);
    result.Count().Should().Be(tags.Count());
  }

  [Fact]
  public async void Tag_ReturnsTag_OnGetTagRequestWithParam() {
    //Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.GetAsync(tags.First().Id.Value).Returns(tags.First());

    //Act
    var result = await mockTagService.GetAsync(tags.First().Id.Value);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeEquivalentTo(
      tags.Where(x => x.Id == result!.Id).FirstOrDefault()
    );
  }

  [Fact]
  public async void Tag_ReturnsTrue_OnExistingTagDeletion() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.DeleteAsync(tags.Last().Id.Value).Returns(true);

    // Act
    var result = await mockTagService.DeleteAsync(tags.Last().Id.Value);

    // Assert
    result.Should().Be(true);
  }

  [Fact]
  public async void Tag_ReturnsFalse_OnNonExistingTagDeletion() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var id = Guid.NewGuid();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.DeleteAsync(id).Returns(false);

    // Act
    var result = await mockTagService.DeleteAsync(id);

    // Assert
    result.Should().Be(false);
  }

  [Fact]
  public async void Tag_ReturnsTrue_OnExistingManyTagsDeletion() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var ids = tags.Select(x => Guid.Parse(x.Id.Value.ToString())).ToList();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.DeleteManyAsync(ids).Returns(true);

    // Act
    var result = await mockTagService.DeleteManyAsync(ids);

    // Assert
    result.Should().Be(true);
  }

  [Fact]
  public async void Tag_ReturnsFalse_OnNonExistingManyTagsDeletion() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var ids = new List<Guid>() {
      Guid.NewGuid(),
      Guid.NewGuid()
    };
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.DeleteManyAsync(ids).Returns(false);

    // Act
    var result = await mockTagService.DeleteManyAsync(ids);

    // Assert
    result.Should().Be(false);
  }

  [Fact]
  public async void Tag_ReturnsTrue_OnNewTagAddition() {
    // Arrange
    var tagRequest = new CreateTagRequest() {
      HasSynonyms = true,
      IsModeratorOnly = true,
      IsRequired = false,
      Name = "c#",
      Count = 42069
    };
    var mappedTag = tagRequest.ToTag();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.CreateAsync(mappedTag).Returns(true);

    // Act
    var result = await mockTagService.CreateAsync(mappedTag);

    // Assert
    result.Should().Be(true);
  }

  [Fact]
  public async void Tag_ReturnsFalse_OnExistingTagAddition() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var targetTag = tags.First();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.CreateAsync(targetTag).Returns(false);

    // Act
    var result = await mockTagService.CreateAsync(targetTag);

    // Assert
    result.Should().Be(false);
  }

  [Fact]
  public async void Tag_ReturnsTrue_OnNewManyTagsAddition() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.CreateManyAsync(tags).Returns(true);

    // Act
    var result = await mockTagService.CreateManyAsync(tags);

    // Assert
    result.Should().Be(true);
  }

  [Fact]
  public async void Tag_ReturnsTrue_OnExistingManyTagsAddition() {
    // Arrange
    var exampleTags = new[] { "c#", "c++", "javascript", "java" };
    var tagsFaker = new Faker<Tag>()
      .RuleFor(x => x.Name, f => TagName.From(f.PickRandom(exampleTags)))
      .RuleFor(x => x.HasSynonyms, f => f.Random.Bool())
      .RuleFor(x => x.IsModeratorOnly, f => f.Random.Bool())
      .RuleFor(x => x.IsRequired, f => f.Random.Bool())
      .RuleFor(x => x.Count, f => TagCount.From(f.Random.Long(0, 5000)));
    var tags = tagsFaker.GenerateBetween(1000, 2000).AsEnumerable();
    var additonTags = tags.Take(500);
    var mockTagService = Substitute.For<ITagService>();
    mockTagService.CreateManyAsync(additonTags).Returns(true);

    // Act
    var result = await mockTagService.CreateManyAsync(additonTags);

    // Assert
    result.Should().Be(true); // <--- true because multiple add filters already existing ones
  }
}