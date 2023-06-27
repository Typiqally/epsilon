using Epsilon.Abstractions.Service;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
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

    [HttpGet("accessible-students")]
    public async Task<IEnumerable<User>> GetAccessibleStudents()
    {
        return await _filterService.GetAccessibleStudents();
    }
}