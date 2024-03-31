using StackAPI.Models.Data;

namespace StackAPI.Repositories;
public interface ITagRepository {
  Task<bool> CreateAsync(TagDto tag);
  Task<bool> CreateManyAsync(IList<TagDto> tags);
  Task<TagDto?> GetAsync(Guid id);
  Task<TagDto?> GetAsync(string name);
  Task<IEnumerable<TagDto>> GetManyAsync(IList<Guid> ids);
  Task<IEnumerable<TagDto>> GetManyAsync(IList<string> names);
  Task<IEnumerable<TagDto>> GetAllAsync();
  Task<IEnumerable<TagDto>> GetAllAsync(TagDtoMeta tagDtoMeta);
  Task<bool> DeleteAsync(Guid id);
  Task<bool> DeleteManyAsync(IList<Guid> ids);
  Task<TagSumCountDto> GetTotalCountAsync();
  Task<TagsTotalDto> GetRecordsNumber();
}
