namespace StackAPI.Endpoints;
public static class EndpointExtensions {
  public static void
    AddEndpoints(this IServiceCollection services, params Type[] markers) {
    var endpoints = new List<IEndpoint>();
    foreach (var marker in markers) {
      endpoints.AddRange(marker.Assembly.ExportedTypes
        .Where(x => typeof(IEndpoint).IsAssignableFrom(x) && !x.IsAbstract)
        .Select(Activator.CreateInstance).Cast<IEndpoint>());
    }

    foreach (var endpoint in endpoints) {
      endpoint.DefineServices(services);
    }

    services.AddSingleton(endpoints as IReadOnlyCollection<IEndpoint>);
  }

  public static void UseEndpoints(this WebApplication app) {
    var endpoints = app.Services.GetRequiredService<IReadOnlyCollection<IEndpoint>>();

    foreach (var endpoint in endpoints) {
      endpoint.DefineEndpoints(app);
    }
  }
}