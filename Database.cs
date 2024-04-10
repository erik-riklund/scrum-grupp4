using MongoDB.Driver;
using MongoDB.Entities;

namespace App
{
  public class DatabaseSettings
  {
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
  }

  public class DatabaseService(DatabaseSettings settings)
  {
    public async Task InitAsync() => await DB.InitAsync(settings.DatabaseName,
      MongoClientSettings.FromConnectionString(settings.ConnectionString)
    );
  }
}