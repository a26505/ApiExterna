using System.Net.Http.Json;
using Models;

namespace Repositories;

public class UsersRepository
{
    private readonly HttpClient _http;
    public UsersRepository(HttpClient http) => _http = http;

    public async Task<List<User>> GetUsersAsync()
        => await _http.GetFromJsonAsync<List<User>>("users") ?? new();
}
