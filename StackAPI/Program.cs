using StackAPI.Database;
using StackAPI.Endpoints;
using StackAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints(typeof(IEndpoint));

builder.Services.AddCors(options => {
  options.AddPolicy(name: CorsString, builder => {
    builder.WithOrigins(
      $"http://{config["Cors:AllowedOrigin"]}",
      $"https://{config["Cors:AllowedOrigin"]}"
    );
  });
});

var connString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (string.IsNullOrEmpty(connString)) {
  connString = config["Database:ConnectionString"];
}

if (string.IsNullOrEmpty(connString)) {
  throw new ArgumentException("Connection string is null", nameof(connString));
}

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new PostgreSQLConnectionFactory(connString!));
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction()) {
  app.UseHttpsRedirection();
}
app.UseRouting();
app.UseCors(CorsString);
app.UseEndpoints();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
var stackConsumerService = app.Services.GetRequiredService<IStackApiConsumerService>();

await databaseInitializer.DropAsync();
await databaseInitializer.InitializeAsync();

if (!app.Environment.IsStaging()) {
  await stackConsumerService.PopulateDb();
}

app.Run();

public partial class Program {
  public const string CorsString = "corsString";
}