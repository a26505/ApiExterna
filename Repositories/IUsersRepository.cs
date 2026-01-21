using Models;

namespace Repositories;

public interface IUsersRepository
{
    Task<List<User>> GetUsersAsync();
}