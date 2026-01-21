using Models;

namespace Repositories;
public interface ITasksRepository
{
    Task<List<TaskModel>> GetTasksAsync();
}