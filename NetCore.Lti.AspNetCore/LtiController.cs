using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NetCore.Lti.AspNetCore;

[ApiController]
[Route("/lti")]
public class LtiController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ILtiTokenValidator _tokenValidator;
    private readonly IToolPlatformService _toolPlatformService;
    private readonly LtiOptions _ltiOptions;

    public LtiController(
        ILogger<LtiController> logger,
        ILtiTokenValidator tokenValidator,
        IToolPlatformService toolPlatformService,
        IOptions<LtiOptions> options)
    {
        _logger = logger;
        _tokenValidator = tokenValidator;
        _toolPlatformService = toolPlatformService;
        _ltiOptions = options.Value;
    }

    [HttpPost("oidc/auth")]
    public async Task<IActionResult> LaunchOidcAuth([ModelBinder(typeof(LtiOpenIdConnectLaunchModelBinder))] LtiOpenIdConnectLaunch launchRequest)
    {
        var origin = Request.Headers.Origin.SingleOrDefault();

        // Currently there is only support for Canvas LMS
        if (launchRequest.Issuer != CanvasConstants.Issuer || origin == null)
        {
            return BadRequest("Invalid issuer");
        }

        var authorizeUrl = new Uri(new Uri(origin), CanvasConstants.AuthorizationEndpoint);
        var hostUrl = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}");

        var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var authorizeRedirectUrl = launchRequest.CreateAuthorizeUrl(
            authorizeUrl,
            new Uri(hostUrl, _ltiOptions.RedirectUri),
            nonce
        );

        _logger.LogDebug("Redirecting launch to {AuthorizeUrl}", authorizeRedirectUrl);

        HttpContext.Session.SetString("nonce", nonce);

        return Redirect(authorizeRedirectUrl);
    }

    [HttpPost("oidc/callback")]
    public async Task<IActionResult> ProcessOidcCallback([ModelBinder(typeof(LtiOpenIdCallbackLaunchModelBinder))] LtiOpenIdConnectCallback callback)
    {
        var message = new LtiRequest(callback.IdToken);
        var platformReference = message.ToolPlatform;
        var platform = await _toolPlatformService.GetById(platformReference.Id);

        var signatureResult = await _tokenValidator.ValidateSignature(platform, message);
        if (signatureResult == null)
        {
            return BadRequest("Unable to verify identity token");
        }

        var nonce = message.Payload.Nonce;
        if (nonce == null || !Equals(nonce, HttpContext.Session.GetString("nonce")))
        {
            return BadRequest("Nonce mismatch");
        }

        var targetLinkUri = message.TargetLinkUri;
        if (targetLinkUri == null)
        {
            return BadRequest("No target link uri found in identity token claims");
        }

        HttpContext.Session.SetString("id_token", callback.IdToken);

        return Redirect(targetLinkUri.ToString());
    }
}