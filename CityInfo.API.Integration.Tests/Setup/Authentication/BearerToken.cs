using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CityInfo.API.Integration.Tests.Setup.Authentication;

public static class BearerToken
{
    public static async Task GetBearerToken(this HttpClient client, string email, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            Email = email,
            Password = password
        }), Encoding.UTF8, "application/json");
        
        var token = await client.PostAsync("api/authentication/authenticate", content);

        if (!token.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("You provided incorrect details to get bearer token.");
        }
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await token.Content.ReadAsStringAsync());
    }
}