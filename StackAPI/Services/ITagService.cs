using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;

namespace StackAPI.Services;
public interface ITagService {
  Task<bool> CreateAsync(Tag tag);
  Task<bool> CreateManyAsync(IEnumerable<Tag> tags);
  Task<Tag?> GetAsync(Guid id);
  Task<Tag?> GetByNameAsync(TagName tagName);
  Task<IEnumerable<Tag>> GetAllAsync();
  Task<IEnumerable<Tag>> GetAllAsync(TagMeta tagMeta);
  Task<bool> DeleteAsync(Guid id);
  Task<bool> DeleteManyAsync(IEnumerable<Guid> ids);
  Task<TagShare> GetTagShareAsync(TagName tagName);
  Task<TagsTotal> GetTotalTagsNumber();
}
