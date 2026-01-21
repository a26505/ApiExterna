using DTOs;
using QueryParams;

namespace Services;

public interface ISummaryService
{
    Task<SummaryDto> GetSummaryAsync();
}