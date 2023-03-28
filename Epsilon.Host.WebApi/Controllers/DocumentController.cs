using Epsilon.Abstractions.Model;
using Epsilon.Export.Exporters;
using Epsilon.Host.WebApi.Models;
using Epsilon.Host.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("api/document/{documentId:int}/")]
public class DocumentController : Controller
{
    private static int GetRandomNumber()
    {
        return new Random().Next(0, 10);
    }

    [HttpGet("structure")]
    public IActionResult GetDocumentStructure(int documentId)
    {
        return Ok(new GetDocumentStructureResponse
        {
            DocumentId = documentId,
            ComponentIds = new[] { GetRandomNumber(), GetRandomNumber(), GetRandomNumber() }
        });
    }
    
    [HttpGet("word")]
    public async Task<IActionResult> GetWord(int documentId)
    {
        var wordExporter = new WordModuleExporter();
        var wordContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        var stream = await wordExporter.Export(new ExportData
        {
            CourseModules = new []
            {
                new CourseModule
                {
                    Name = "Module 1",
                    Outcomes = new List<CourseOutcome>
                    {
                        new CourseOutcome
                        {
                            Name = "Outcome 1",
                            Description = "Description 1",
                            Assignments = new List<CourseAssignment>
                            {
                                new CourseAssignment
                                {
                                    Name = "Assignment 1",
                                    Url = "https://google.com/",
                                    Score = "Outstanding"
                                }
                            }
                        }
                    }
                }
            }
        }, "word");
        
        // Reset position to zero to prepare for copy
        stream.Position = 0;

        return File(stream, wordContentType, "CompetenceDocument.docx");
    }
    
    [HttpGet("home-page")]
    public IActionResult GetHomePage(int documentId)
    {
        return Ok(new GetDocumentHomePageResponse
        {
            DocumentId = documentId,
            HomePage = new HomePage
            {
                Id = 2,
                Type = "homepage",
                Title = "Homepage",
                Description = "The homepage of the website",
                Config = new Dictionary<string, string>
                {
                    { "HomePageRecord", "Jelle" },
                },
                Data = new HomePageData
                {
                    SomeHomePageDataArray = new[] { "red", "green", "blue" }
                }
            }
        });
    }

    [HttpGet("kpi-matrix")]
    public IActionResult GetKpiMatrix(int documentId)
    {
        return Ok(new GetDocumentKpiMatrixResponse
        {
            DocumentId = documentId,
            KpiMatrix = new KpiMatrix
            {
                Id = 1,
                Type = "kpi-matrix",
                Title = "KPI Matrix",
                Description = "A matrix of KPIs",
                Config = new Dictionary<string, string>
                {
                    { "KpiConfigRecord", "Sven" },
                },
                Data = new KpiMatrixData
                {
                    SomeKpiMatrixDataArray = new[] { "red", "green", "blue" }
                }
            },
        });
    }
}