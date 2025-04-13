using System.Text.Json.Serialization;

namespace IDMToolsEX.Models;

public class Settings
{
    public DatabaseCredentials DatabaseCredentials { get; set; } = new();
}

public class DatabaseCredentials
{
    [JsonPropertyName("database")] public string Database { get; set; } = "pos";
    [JsonPropertyName("server")] public string Server { get; set; } = "localhost";
    [JsonPropertyName("port")] public int Port { get; set; } = 3306;
    [JsonPropertyName("username")] public string Username { get; set; } = "root";
    [JsonPropertyName("password")] public string Password { get; set; } = "135321";
}
