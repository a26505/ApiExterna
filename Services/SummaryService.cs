using DTOs;
using Models;

namespace Services;

public class SummaryService
{
    private readonly UsersService _users;
    private readonly TasksService _tasks;
    private readonly ApiDbContext _db;

    public SummaryService(UsersService users, TasksService tasks, ApiDbContext db)
    {
        _users = users;
        _tasks = tasks;
        _db = db;
    }

    public async Task<SummaryDto> GetSummaryAsync()
    {
        var users = await _users.GetUsersAsync(new());
        var tasks = await _tasks.GetTasksAsync(new());

        _db.Logs.Add(new Log
        {
            Endpoint = "/api/summary",
            Message = "Summary endpoint called"
        });

        await _db.SaveChangesAsync();

        return new SummaryDto
        {
            TotalUsers = users.Count,
            TotalTasks = tasks.Count
        };
    }
}
