using DTOs;
using Models;

namespace Services;

public class SummaryService : ISummaryService
{
    private readonly IUsersService _users;
    private readonly ITasksService _tasks;
    private readonly ApiDbContext _db;
    private readonly ILogger<SummaryService> _logger;

    public SummaryService(IUsersService users, ITasksService tasks, ApiDbContext db, ILogger<SummaryService> logger)
    {
        _users = users;
        _tasks = tasks;
        _db = db;
        _logger = logger;
    }

    public async Task<SummaryDto> GetSummaryAsync()
    {
        // 1. Obtenemos datos de las APIs externas
        var users = await _users.GetUsersAsync(new());
        var tasks = await _tasks.GetTasksAsync(new());

        // 2. Intentamos guardar el Log en MySQL con Try-Catch
        try
        {
            _db.Logs.Add(new Log
            {
                Endpoint = "/api/summary",
                Message = $"Se procesaron {users.Count} usuarios y {tasks.Count} tareas.",
                CalledAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"No se pudo guardar el log en MySQL: {ex.Message}");
            // No lanzamos la excepción para que el usuario reciba su respuesta aunque falle el log
        }

        return new SummaryDto
        {
            TotalUsers = users.Count,
            TotalTasks = tasks.Count
        };
    }
}