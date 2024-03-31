﻿using Testcontainers.PostgreSql;

namespace StackAPI.Tests.Integration.Helpers;
public class DatabaseFixture : IAsyncLifetime {
  private readonly PostgreSqlContainer container =
        new PostgreSqlBuilder()
            .Build();
  public string ConnectionString => container.GetConnectionString();
  public string ContainerId => $"{container.Id}";
  public Task InitializeAsync()
      => container.StartAsync();
  public Task DisposeAsync()
      => container.DisposeAsync().AsTask();
}
