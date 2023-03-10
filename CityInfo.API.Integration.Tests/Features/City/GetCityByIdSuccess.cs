using System.Net;
using System.Text.Json;
using CityInfo.API.Integration.Tests.Setup.Authentication;
using CityInfo.API.Models.Responses.Cities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CityInfo.API.Integration.Tests.Features.City;

public class GetCityByIdSuccess : IAsyncLifetime
{
#pragma warning disable CS8618
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;
    private HttpResponseMessage _response;
    private CityDto? _city;
#pragma warning disable CS8618

    public async Task InitializeAsync()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();

        await _client.GetBearerToken("mycrofth@britain.uk", "123456");

        const string requestUri = "api/cities/fc1c76c4-3e26-4e90-9a07-35d42c9c2d74";
        _response = await _client.GetAsync(requestUri);
        
        var city = await _response.Content.ReadAsStringAsync();
        _city = JsonSerializer.Deserialize<CityDto>(city, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    [Fact]
    public void Should_BeSuccess()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        _city.Should().NotBeNull().And.BeOfType<CityDto>();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        _response.Dispose();
        await _webApplicationFactory.DisposeAsync();
    }
}