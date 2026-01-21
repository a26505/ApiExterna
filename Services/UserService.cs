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

    // SOLO filtra si el usuario envió un nombre. Si no, pasa de largo y devuelve todo.
    if (!string.IsNullOrEmpty(query.Name))
    {
        users = users.Where(u => u.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    return users.Select(u => new UserDto { 
        Id = u.Id, 
        Name = u.Name, 
        Email = u.Email 
    }).ToList();
}
    
}