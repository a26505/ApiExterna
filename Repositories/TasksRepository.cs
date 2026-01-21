using System.Net.Http.Json;
using Models;

namespace Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly HttpClient _http;
    private readonly ILogger<TasksRepository> _logger;

    public TasksRepository(HttpClient http, ILogger<TasksRepository> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<TaskModel>>("todos") ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al consumir API externa de Tareas: {ex.Message}");
            return new List<TaskModel>();
        }
    }
}