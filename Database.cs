using MongoDB.Driver;
using MongoDB.Entities;

namespace App
{
  public class DatabaseSettings
  {
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
  }

  public class DatabaseService
  {
    private readonly DatabaseSettings Settings;

    public DatabaseService(DatabaseSettings settings) => Settings = settings;

    public async Task InitAsync() => await DB.InitAsync(Settings.DatabaseName,
      MongoClientSettings.FromConnectionString(Settings.ConnectionString)
    );
  }
}