using DTOs;
using Repositories;
using QueryParams;

namespace Services;

public class TasksService : ITasksService
{
    private readonly ITasksRepository _repo;
    public TasksService(ITasksRepository repo) => _repo = repo;
public async Task<List<TaskDto>> GetTasksAsync(TaskQueryParams query)
{
    var tasks = await _repo.GetTasksAsync();

    // SOLO filtra si el usuario envió un UserId. Si no, devuelve la lista completa.
    if (query.UserId.HasValue)
    {
        tasks = tasks.Where(t => t.UserId == query.UserId.Value).ToList();
    }

    return tasks.Select(t => new TaskDto { 
        Id = t.Id, 
        Title = t.Title, 
        Completed = t.Completed, 
        UserId = t.UserId 
    }).ToList();
}
}