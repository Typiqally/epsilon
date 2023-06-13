using Epsilon.Abstractions.Service;
using Epsilon.Canvas.Abstractions.Model;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilterController : Controller
{
    private readonly IFilterService _filterService;

    public FilterController(IFilterService filterService)
    {
        _filterService = filterService;
    }

    [HttpGet("participated-terms")]
    public async Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms()
    {
        return await _filterService.GetParticipatedTerms();
    }
}