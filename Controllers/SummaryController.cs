using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("api/summary")]
public class SummaryController : ControllerBase
{
    private readonly ISummaryService _service;

    // Inyectamos la Interfaz, no la clase concreta
    public SummaryController(ISummaryService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetSummaryAsync());
}