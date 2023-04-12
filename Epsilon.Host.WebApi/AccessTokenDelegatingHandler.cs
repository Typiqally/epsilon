using System.Net;
using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Epsilon.Host.WebApi;

public class AccessTokenDelegatingHandler : DelegatingHandler
{
    private readonly string _scheme;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenClient _tokenClient;

    public AccessTokenDelegatingHandler(string scheme, IHttpContextAccessor httpContextAccessor, TokenClient tokenClient)
    {
        _scheme = scheme;
        _httpContextAccessor = httpContextAccessor;
        _tokenClient = tokenClient;
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext should not be null");
        }

        var authenticationInfo = await httpContext.AuthenticateAsync(_scheme);
        if (authenticationInfo.Properties == null)
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        var token = authenticationInfo.Properties.GetTokenValue("access_token");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            var refreshToken = authenticationInfo.Properties!.GetTokenValue("refresh_token");
            var tokenResponse = await _tokenClient.RequestRefreshTokenAsync(refreshToken, cancellationToken: cancellationToken);

            authenticationInfo.Properties!.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authenticationInfo.Properties!.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);
            authenticationInfo.Properties!.UpdateTokenValue("expires_in", tokenResponse.ExpiresIn.ToString());

            // Submit changes
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationInfo.Principal, authenticationInfo.Properties);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }
}