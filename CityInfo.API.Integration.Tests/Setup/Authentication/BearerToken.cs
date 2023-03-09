using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CityInfo.API.Models.Requests.Authentication;

namespace CityInfo.API.Integration.Tests.Setup.Authentication;

public static class BearerToken
{
    private static string Bearer => "Bearer";

    public static async Task GetBearerToken(this HttpClient client, string email, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new AuthenticateRequestBody
        {
            Email = email,
            Password = password
        }), Encoding.UTF8, "application/json");

        const string requestUri = "api/authentication/authenticate";
        var token = await client.PostAsync(requestUri, content);

        if (!token.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("You provided incorrect details to get bearer token.");
        }
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(Bearer, await token.Content.ReadAsStringAsync());
    }
}