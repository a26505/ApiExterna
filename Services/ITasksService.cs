using DTOs;
using QueryParams;

namespace Services;

public interface ITasksService
{
    Task<List<TaskDto>> GetTasksAsync(TaskQueryParams query);
}