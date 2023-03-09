using System.Net;
using System.Text.Json;
using CityInfo.API.Models.Responses;
using CityInfo.API.Models.Responses.Cities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CityInfo.API.Integration.Tests.Features.Cities;

public class GetCitiesSuccess : IAsyncLifetime
{
#pragma warning disable CS8618
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;
    private HttpResponseMessage _response;
    private IEnumerable<CityDto>? _cities;
#pragma warning restore CS8618

    public async Task InitializeAsync()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();

        const string requestUri = "api/cities?pageNumber=1&pageSize=5";
        _response = await _client.GetAsync(requestUri);
        
        var city = await _response.Content.ReadAsStringAsync();
        _cities = JsonSerializer.Deserialize<IEnumerable<CityDto>>(city, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    [Fact]
    public void Should_Be_Success()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        _cities?.Count().Should().Be(3);
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        _response.Dispose();
        await _webApplicationFactory.DisposeAsync();
    }
}