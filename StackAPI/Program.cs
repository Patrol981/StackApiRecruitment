using StackAPI.Database;
using StackAPI.Endpoints;

const string CorsString = "corsString";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints(typeof(IEndpoint));
builder.Services.AddCors(options => {
  options.AddPolicy(name: CorsString, builder => {
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
  });
});

var config = builder.Configuration;

var connString = config["Database:ConnectionString"];
if (connString == null) {
  throw new ArgumentException("Connection string is null", nameof(connString));
}

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new PostgreSQLConnectionFactory(connString));
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(CorsString);

app.UseEndpoints();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();

public partial class Program { }