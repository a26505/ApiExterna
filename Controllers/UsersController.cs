using Microsoft.AspNetCore.Mvc;
using QueryParams;
using Services;

namespace Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;

    public UsersController(IUsersService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] UserQueryParams query)
    {
        // Si el 'query.Name' es null, el servicio devolverá todos los usuarios automáticamente
        var users = await _service.GetUsersAsync(query);
        return Ok(users);
    }
}