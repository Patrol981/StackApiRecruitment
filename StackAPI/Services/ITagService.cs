using StackAPI.Models.Domain;

namespace StackAPI.Services;
public interface ITagService {
  Task<bool> CreateAsync(Tag tag);
  Task<Tag?> GetAsync(Guid id);
  Task<IEnumerable<Tag>> GetAllAsync();
  Task<bool> DeleteAsync(Guid id);
}
