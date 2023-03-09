using System.Net;
using CityInfo.API.Integration.Tests.Setup.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CityInfo.API.Integration.Tests.Features.City;

public class GetCityByIdForbidden : IAsyncLifetime
{
#pragma warning disable CS8618
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;
    private HttpResponseMessage _response;
#pragma warning disable CS8618

    public async Task InitializeAsync()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();

        await _client.GetBearerToken("mpenury@femblem.jp", "123456");

        const string requestUri = "api/cities/fc1c76c4-3e26-4e90-9a07-35d42c9c2d74";
        _response = await _client.GetAsync(requestUri);
    }

    [Fact]
    public void Should_BeForbidden()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        _response.Dispose();
        await _webApplicationFactory.DisposeAsync();
    }
}