using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("api/summary")]
public class SummaryController : ControllerBase
{
    private readonly SummaryService _service;
    public SummaryController(SummaryService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetSummaryAsync());
}
