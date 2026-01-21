using System.Net.Http.Json;
using Models;

namespace Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly HttpClient _http;
    private readonly ILogger<UsersRepository> _logger;

    public UsersRepository(HttpClient http, ILogger<UsersRepository> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<User>>("users") ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al consumir API de Usuarios: {ex.Message}");
            return new List<User>(); // Devolvemos lista vacía para no romper el flujo
        }
    }
}