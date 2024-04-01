using FluentValidation;
using FluentValidation.Results;

using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;
using StackAPI.Models.Mapping;
using StackAPI.Repositories;

namespace StackAPI.Services;
public class TagService : ITagService {
  private readonly ITagRepository _tagRepository;
  private readonly ILogger<ITagService> _logger;

  public TagService(ITagRepository tagRepository, ILogger<ITagService> logger) {
    _tagRepository = tagRepository;
    _logger = logger;
  }

  public async Task<bool> CreateAsync(Tag tag) {
    var existingTag = await _tagRepository.GetAsync(tag.Name.Value);
    if (existingTag != null) {
      var msg = $"Tag {tag.Name.Value} already exists";
      _logger.LogError(msg, tag);
      throw new ValidationException(msg, new[] {
        new ValidationFailure(nameof(Tag), msg)
      });
    }

    _logger.LogInformation("Creating tag");
    var tagDto = tag.ToTagDto();
    return await _tagRepository.CreateAsync(tagDto);
  }

  public async Task<bool> CreateManyAsync(IEnumerable<Tag> tags) {
    // filter out duplicates
    var uniqueTags = tags
      .GroupBy(tag => tag.Name)
      .Select(group => group.First())
      .ToList();

    var tagNames = uniqueTags.Select(tag => tag.Name.Value).ToList();
    var existingTags = (await _tagRepository.GetManyAsync(tagNames)).ToList();

    var tagsDto = uniqueTags.ToTagDto();
    var tagsToCreate = tagsDto
      .Where(tag => !existingTags.Any(existingTag => existingTag.Name == tag.Name))
      .ToList();

    _logger.LogInformation("Creating many tags");
    return await _tagRepository.CreateManyAsync(tagsToCreate);
  }

  public async Task<bool> DeleteAsync(Guid id) {
    _logger.LogInformation("Deleting tag");
    return await _tagRepository.DeleteAsync(id);
  }

  public async Task<bool> DeleteManyAsync(IEnumerable<Guid> ids) {
    _logger.LogInformation("Deleting many tags");
    return await _tagRepository.DeleteManyAsync(ids.ToList());
  }

  public async Task<IEnumerable<Tag>> GetAllAsync() {
    _logger.LogInformation("Getting all tags");
    var tagsDto = await _tagRepository.GetAllAsync();
    return tagsDto.Select(x => x.ToTag());
  }

  public async Task<IEnumerable<Tag>> GetAllAsync(TagMeta tagMeta) {
    _logger.LogInformation("Getting all tags filtered");
    var tagsDto = await _tagRepository.GetAllAsync(tagMeta.ToTagDtoMeta());
    return tagsDto.Select(x => x.ToTag());
  }

  public async Task<Tag?> GetAsync(Guid id) {
    _logger.LogInformation("Getting tag by id");
    var tagDto = await _tagRepository.GetAsync(id);
    return tagDto?.ToTag();
  }

  public async Task<Tag?> GetByNameAsync(TagName tagName) {
    _logger.LogInformation("Getting tag by name");
    var tagDto = await _tagRepository.GetAsync(tagName.Value);
    return tagDto?.ToTag();
  }

  public async Task<TagShare> GetTagShareAsync(TagName tagName) {
    _logger.LogInformation("Getting tag share");
    var tag = (await _tagRepository.GetAsync(tagName.Value))?.ToTag();
    var tagShare = new TagShare();
    if (tag == null) {
      _logger.LogWarning("Tag is null");
      return tagShare;
    }

    _logger.LogInformation("Getting total tag count");
    var totalCount = await _tagRepository.GetTotalCountAsync();
    double percentage = (tag.Count.Value / (double)totalCount.TotalCount) * 100;
    tagShare.SharePercentage = (decimal)percentage;
    return tagShare;
  }

  public async Task<TagsTotal> GetTotalTagsNumber() {
    _logger.LogInformation("Getting total tag number");
    return (await _tagRepository.GetRecordsNumber())!.ToTagsTotal();
  }
}
