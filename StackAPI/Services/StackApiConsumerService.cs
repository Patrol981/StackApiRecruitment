using StackAPI.Models.Domain;
using StackAPI.Models.Mapping;
using StackAPI.Models.Responses;
using StackAPI.Utils;

namespace StackAPI.Services;
public class StackApiConsumerService : IStackApiConsumerService {
  private readonly ITagService _tagService;
  private readonly ILogger<IStackApiConsumerService> _logger;
  private readonly HttpClient _httpClient;

  public StackApiConsumerService(
    ITagService tagService,
    ILogger<IStackApiConsumerService> logger,
    HttpClient httpClient
  ) {
    _tagService = tagService;
    _httpClient = httpClient;
    _logger = logger;
  }

  public async Task<GetAllTagsResponse> PopulateDb() {
    _logger.LogInformation("Getting all tags");
    var allTags = await _tagService.GetAllAsync();

    _logger.LogInformation("Deleting all tags");
    await _tagService.DeleteManyAsync(allTags.Select(tag => tag.Id.Value));

    _logger.LogInformation("Fetching data from stack api");
    var itemsToAdd = new List<Tag>();

    var apiKey = Environment.GetEnvironmentVariable("API_KEY");
    int pageSize = StringUtils.IsNullOrEmpty(apiKey) ? 100 : 30;
    int operationsNumber = StringUtils.IsNullOrEmpty(apiKey) ? 18 : 1;

    for (int i = 1; i <= operationsNumber; i++) {
      var fetchResult = await _httpClient.GetAsync(
        $"tags?order=desc&sort=popular&site=stackoverflow&filter=!9WL3B-Bjh&pagesize={pageSize}&page={i}&key={apiKey}"
      );

      if (!fetchResult.IsSuccessStatusCode) {
        throw new BadHttpRequestException("unable to fetch data from stack server");
      }

      var json = await fetchResult.Content.ReadFromJsonAsync<StackApiTagsResponse>();

      itemsToAdd.AddRange([.. json?.ToTag()]);
    }

    _logger.LogInformation("Creating tags based on api response");
    var isPopulated = await _tagService.CreateManyAsync(itemsToAdd);

    return itemsToAdd.ToGetAllTagsResponse();
  }
}
