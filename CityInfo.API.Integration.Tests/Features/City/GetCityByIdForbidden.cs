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

        _response = await _client.GetAsync("api/cities/FC1C76C4-3E26-4E90-9A07-35D42C9C2D74");
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