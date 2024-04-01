
using Microsoft.AspNetCore.Http.HttpResults;

using StackAPI.Services;

namespace StackAPI.Endpoints;
public class StackInteractionEndpoints : IEndpoint {
  public void DefineEndpoints(WebApplication app) {
    app.MapGet("populateDb", PopulateDb);
  }

  public void DefineServices(IServiceCollection services) {
    services.AddSingleton<IStackApiConsumerService, StackApiConsumerService>();
    services.AddHttpClient<IStackApiConsumerService, StackApiConsumerService>()
      .ConfigurePrimaryHttpMessageHandler(() => {
        return new HttpClientHandler {
          AutomaticDecompression = System.Net.DecompressionMethods.GZip |
                                   System.Net.DecompressionMethods.Deflate
        };
      })
      .ConfigureHttpClient((serviceProvider, client) => {
        client.BaseAddress = new Uri("https://api.stackexchange.com/2.3/");
      });
  }

  public static async Task<Results<Ok<string>, ProblemHttpResult, Conflict<string>>> PopulateDb(
    IStackApiConsumerService stackService
  ) {
    try {
      var isAdded = await stackService.PopulateDb();
      return isAdded ?
        TypedResults.Ok("Successfully populated db.") :
        TypedResults.Conflict("Could not add items to database.");

    } catch (Exception ex) {
      return TypedResults.Problem(ex.Message);
    }

  }
}
