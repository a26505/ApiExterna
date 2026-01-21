using Microsoft.AspNetCore.Mvc;
using QueryParams;
using Services;
namespace Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UsersService _service;
    public UsersController(UsersService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] UserQueryParams query)
        => Ok(await _service.GetUsersAsync(query));
}
