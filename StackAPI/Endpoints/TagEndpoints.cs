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
    app.MapPost("tags/addMany", CreateManyTags);
    app.MapGet("tags/{tagId}", GetTag);
    app.MapGet("tags/getByName/{tagName}", GetTagByName);
    app.MapGet("tags/{sortBy}/{direction}/{offset}", GetAllTagsSorted);
    app.MapGet("tags", GetAllTagsSorted);
    app.MapDelete("tags/{tagId}", RemoveTag);
    app.MapPost("tags/removeMany", RemoveManyTags);
    app.MapGet("tags/share/{tagName}", GetTagShare);
  }

  public void DefineServices(IServiceCollection services) {
    services.AddSingleton<ITagRepository, TagRepository>();
    services.AddSingleton<ITagService, TagService>();
  }

  public static async Task<Results<Ok<TagResponse>, Conflict<string>>> CreateTag(
    ITagService tagService, CreateTagRequest tagRequest
  ) {
    try {
      var tag = tagRequest.ToTag();

      await tagService.CreateAsync(tag);

      var tagResponse = tag.ToTagResponse();
      return TypedResults.Ok(tagResponse);

    } catch (Exception ex) {
      return TypedResults.Conflict(ex.Message);
    }
  }

  public static async Task<Results<Ok<GetAllTagsResponse>, Conflict<string>>> CreateManyTags(
    ITagService tagService, IList<CreateTagRequest> tagRequests
  ) {
    try {
      var tags = tagRequests.ToTag();

      await tagService.CreateManyAsync(tags);

      var tagsResponse = tags.ToGetAllTagsResponse();
      return TypedResults.Ok(tagsResponse);
    } catch (Exception ex) {
      return TypedResults.Conflict(ex.Message);
    }
  }

  public static async Task<Results<Ok<TagResponse>, NotFound, ProblemHttpResult>> GetTag(
    ITagService tagService, GetTagRequest tagId
  ) {
    try {
      var tag = await tagService.GetAsync(tagId.Id);

      if (tag == null) {
        return TypedResults.NotFound();
      }

      var tagResponse = tag.ToTagResponse();
      return TypedResults.Ok(tagResponse);

    } catch (Exception ex) {
      return TypedResults.Problem(ex.Message);
    }
  }

  public static async Task<Results<Ok<TagResponse>, NotFound, ProblemHttpResult>> GetTagByName(
    ITagService tagService, GetTagNamedRequest tagName
  ) {
    try {
      var tag = await tagService.GetByNameAsync(tagName.ToTagName());

      if (tag == null) {
        return TypedResults.NotFound();
      }

      var tagResponse = tag.ToTagResponse();
      return TypedResults.Ok(tagResponse);

    } catch (Exception ex) {
      return TypedResults.Problem(ex.Message);
    }
  }

  public static async Task<Results<Ok<GetAllTagsResponse>, NotFound, ProblemHttpResult>> GetAllTagsSorted(
    ITagService tagService, string? sortBy, string? direction, int? offset
  ) {
    try {
      var tagRequest = new GetTagSortedRequest() {
        SortBy = sortBy ?? default!,
        SortDirection = direction ?? default!,
        Offset = offset ?? 0
      };
      var tagMeta = tagRequest.ToTagMeta();

      var tags = await tagService.GetAllAsync(tagMeta);
      var tagsTotal = await tagService.GetTotalTagsNumber();

      var tagsResponse = tags.ToGetAllTagsResponse();
      tagsResponse.TotalRecords = tagsTotal.TotalCount;
      tagsResponse.CurrentPage = tagMeta.Offset;
      tagsResponse.PageSize = tagMeta.Limit;
      tagsResponse.TotalPages = tagsTotal.TotalCount / tagsResponse.PageSize;

      return TypedResults.Ok(tagsResponse);

    } catch (Exception ex) {
      return TypedResults.Problem(ex.Message);
    }
  }

  public static async Task<Results<NoContent, NotFound>> RemoveTag(
    ITagService tagService, DeleteTagRequest tagId
  ) {
    var deleted = await tagService.DeleteAsync(tagId.Id);

    return !deleted ? (Results<NoContent, NotFound>)TypedResults.NotFound() :
      TypedResults.NoContent();
  }

  public static async Task<Results<NoContent, ProblemHttpResult>> RemoveManyTags(
    ITagService tagService, IList<DeleteTagRequest> tagRequests
  ) {
    var deleted = await tagService.DeleteManyAsync(tagRequests.Select(tag => tag.Id));

    return !deleted ? TypedResults.Problem("Could not remove tags") : TypedResults.NoContent();
  }

  public static async Task<Results<Ok<GetTagShareResponse>, ProblemHttpResult>> GetTagShare(
    ITagService tagService, GetTagNamedRequest tagName
  ) {
    try {
      var tagShare = await tagService.GetTagShareAsync(tagName.ToTagName());
      return TypedResults.Ok(tagShare.ToGetTagShareResponse());
    } catch (Exception ex) {
      return TypedResults.Problem(ex.Message);
    }

  }
}
