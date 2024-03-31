using StackAPI.Models.Responses;

namespace StackAPI.Services;
public interface IStackApiConsumerService {
  Task<GetAllTagsResponse> PopulateDb();
}
