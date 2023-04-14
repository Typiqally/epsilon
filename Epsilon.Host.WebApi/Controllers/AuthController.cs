using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("challenge")]
    public IActionResult ProcessChallenge()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/auth/callback",
        };

        return Challenge(properties, "canvas-docker");
    }

    [HttpGet("callback")]
    public IActionResult ProcessCallback()
    {
        // Close the login window popup
        return Content("<script>window.close()</script>", MediaTypeNames.Text.Html);
    }
}