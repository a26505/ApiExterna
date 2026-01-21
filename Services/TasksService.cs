using DTOs;
using Repositories;
using QueryParams;

namespace Services;

public class TasksService
{
    private readonly TasksRepository _repo;
    public TasksService(TasksRepository repo) => _repo = repo;

    public async Task<List<TaskDto>> GetTasksAsync(TaskQueryParams query)
    {
        var tasks = await _repo.GetTasksAsync();

        if (query.UserId.HasValue)
            tasks = tasks.Where(t => t.UserId == query.UserId).ToList();

        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Completed = t.Completed,
            UserId = t.UserId
        }).ToList();
    }
}
