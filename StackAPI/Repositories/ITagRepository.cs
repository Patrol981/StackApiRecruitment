using StackAPI.Models.Data;

namespace StackAPI.Repositories;
public interface ITagRepository {
  Task<bool> CreateAsync(TagDto tag);
  Task<TagDto?> GetAsync(Guid id);
  Task<TagDto?> GetAsync(string name);
  Task<IEnumerable<TagDto>> GetAllAsync();
  Task<bool> DeleteAsync(Guid id);
}
