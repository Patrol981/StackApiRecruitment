using Microsoft.AspNetCore.Http.HttpResults;

using StackAPI.Models.Mapping;
using StackAPI.Models.Requests;
using StackAPI.Models.Responses;
using StackAPI.Repositories;
using StackAPI.Services;

namespace StackAPI.Endpoints;
public class TagEndpoints : IEndpoint {
  public void DefineEndpoints(WebApplication app) {
    app.MapGet("special request", () => { return "Plseawwse hire me :3"; });

    app.MapPost("tags", CreateTag);
    app.MapGet("tags/{tagRequest}", GetTag);
    app.MapGet("tags", GetAllTags);
    app.MapDelete("tags", RemoveTag);
  }

  public void DefineServices(IServiceCollection services) {
    services.AddSingleton<ITagRepository, TagRepository>();
    services.AddSingleton<ITagService, TagService>();
  }

  private async Task<Ok<TagResponse>> CreateTag(ITagService tagService, CreateTagRequest tagRequest) {
    var tag = tagRequest.ToTag();

    await tagService.CreateAsync(tag);

    var tagResponse = tag.ToTagResponse();
    return TypedResults.Ok(tagResponse);
  }

  private async Task<Results<Ok<TagResponse>, NotFound>> GetTag(ITagService tagService, GetTagRequest tagRequest) {
    var tag = await tagService.GetAsync(tagRequest.Id);

    if (tag == null) {
      return TypedResults.NotFound();
    }

    var tagResponse = tag.ToTagResponse();
    return TypedResults.Ok(tagResponse);
  }

  private async Task<Ok<GetAllTagsResponse>> GetAllTags(ITagService tagService) {
    var tags = await tagService.GetAllAsync();
    var tagsResponse = tags.ToTagsResponse();
    return TypedResults.Ok(tagsResponse);
  }

  private async Task<Results<NoContent, NotFound>> RemoveTag(ITagService tagService, DeleteTagRequest tagRequest) {
    var deleted = await tagService.DeleteAsync(tagRequest.Id);

    return !deleted ? (Results<NoContent, NotFound>)TypedResults.NotFound() : (Results<NoContent, NotFound>)TypedResults.NoContent();
  }
}
