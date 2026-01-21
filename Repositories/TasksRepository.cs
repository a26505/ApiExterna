using System.Net.Http.Json;
using Models;

namespace Repositories;

public class TasksRepository
{
    private readonly HttpClient _http;
    public TasksRepository(HttpClient http) => _http = http;

    public async Task<List<TaskModel>> GetTasksAsync()
        => await _http.GetFromJsonAsync<List<TaskModel>>("todos") ?? new();
}
