using DTOs;
using QueryParams;

namespace Services;

public interface IUsersService
{
    Task<List<UserDto>> GetUsersAsync(UserQueryParams query);
}