﻿using System.Net.Http.Json;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Http;

namespace Epsilon.Canvas.Service;

public class AccountHttpService : HttpService, IAccountHttpService
{
    public AccountHttpService(HttpClient client)
        : base(client)
    {
    }

    public async Task<IEnumerable<EnrollmentTerm>?> GetAllTerms(int accountId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/accounts/{accountId}/terms?per_page=100");
        var response = await Client.SendAsync(request);
        var collection = await response.Content.ReadFromJsonAsync<EnrollmentTermCollection>();

        return collection?.Terms;
    }
}