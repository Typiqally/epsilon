using Epsilon.Abstractions.Model;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("component")]
public class ComponentController : ControllerBase
{
    [HttpGet("competence_profile")]
    public ActionResult<CompetenceProfile> GetCompetenceProfile([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var competenceProfileOutcomes = new List<CompetenceProfileOutcome>();
        for (var i = 0; i < 5; i++)
        {
            competenceProfileOutcomes.Add(GetRandomCompetenceProfileOutcome());
        }
        
        return new CompetenceProfile(
            HboIDomain.HboIDomain2018,
            competenceProfileOutcomes
        );
    }
    
    private static CompetenceProfileOutcome GetRandomCompetenceProfileOutcome()
    {
        return new CompetenceProfileOutcome(
            GetRandom(HboIDomain.HboIDomain2018.ArchitectureLayers),
            GetRandom(HboIDomain.HboIDomain2018.Activities),
            GetRandom(HboIDomain.HboIDomain2018.MasteryLevels),
            GetRandom(new[] { 0, 3, 4, 5 }),
            DateTime.Now
        );
    }
    
    private static T GetRandom<T>(IEnumerable<T> items)
    {
        var random = new Random();
        var itemsArray = items.ToArray();
        var index = random.Next(0, itemsArray.Length);
        return itemsArray.ElementAt(index);
    }
}