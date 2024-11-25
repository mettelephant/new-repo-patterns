using System.Text.Json;

namespace newsetup.repos.ApiService.Repository.Entities.Configurations;

internal static class DatabaseJsonOptions
{
    internal static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}