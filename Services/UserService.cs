using DTOs;
using Repositories;
using QueryParams;

namespace Services;

public class UsersService
{
    private readonly UsersRepository _repo;
    public UsersService(UsersRepository repo) => _repo = repo;

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
