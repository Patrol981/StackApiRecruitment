using FluentValidation;
using FluentValidation.Results;

using StackAPI.Models.Domain;
using StackAPI.Models.Mapping;
using StackAPI.Repositories;

namespace StackAPI.Services;
public class TagService : ITagService {
  private readonly ITagRepository _tagRepository;

  public TagService(ITagRepository tagRepository) {
    _tagRepository = tagRepository;
  }

  public async Task<bool> CreateAsync(Tag tag) {
    var existingTag = await _tagRepository.GetAsync(tag.Name.Value);
    if (existingTag != null) {
      var msg = $"Tag {tag.Name.Value} already exists";
      throw new ValidationException(msg, new[] {
        new ValidationFailure(nameof(Tag), msg)
      });
    }

    var tagDto = tag.ToTagDto();
    return await _tagRepository.CreateAsync(tagDto);
  }

  public async Task<bool> DeleteAsync(Guid id) {
    return await _tagRepository.DeleteAsync(id);
  }

  public async Task<IEnumerable<Tag>> GetAllAsync() {
    var tagsDto = await _tagRepository.GetAllAsync();
    return tagsDto.Select(x => x.ToTag());
  }

  public async Task<Tag?> GetAsync(Guid id) {
    var tagDto = await _tagRepository.GetAsync(id);
    return tagDto?.ToTag();
  }
}
