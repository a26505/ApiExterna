using DTOs;
using Repositories;
using QueryParams;

namespace Services;

public class UserService : IUsersService
{
    private readonly IUsersRepository _repo;
    public UserService(IUsersRepository repo) => _repo = repo;

    public async Task<List<UserDto>> GetUsersAsync(UserQueryParams query)
    {
        var users = await _repo.GetUsersAsync();

        if (!string.IsNullOrEmpty(query.Name))
            users = users.Where(u => u.Name.Contains(query.Name)).ToList();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }).ToList();
    }
}