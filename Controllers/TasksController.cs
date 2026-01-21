using Microsoft.AspNetCore.Mvc;
using Services;
using QueryParams;

namespace Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _service;

    public TasksController(ITasksService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TaskQueryParams query)
    {
        // Si 'query.UserId' no tiene valor, el servicio devolverá la lista completa de tareas
        var tasks = await _service.GetTasksAsync(query);
        return Ok(tasks);
    }
}