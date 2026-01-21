using Microsoft.AspNetCore.Mvc;
using Services;
using QueryParams;

namespace Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TasksService _service;
    public TasksController(TasksService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TaskQueryParams query)
        => Ok(await _service.GetTasksAsync(query));
}
