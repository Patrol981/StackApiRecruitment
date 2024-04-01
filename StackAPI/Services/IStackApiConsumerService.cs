namespace StackAPI.Services;
public interface IStackApiConsumerService {
  Task<bool> PopulateDb();
}
