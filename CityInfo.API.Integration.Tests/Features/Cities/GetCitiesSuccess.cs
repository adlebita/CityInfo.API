using Microsoft.AspNetCore.Mvc.Testing;

namespace CityInfo.API.Integration.Tests.Features.Cities;

public class GetCitiesSuccess : IAsyncLifetime
{
#pragma warning disable CS8618
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;
    private HttpResponseMessage _response;
#pragma warning restore CS8618

    public async Task InitializeAsync()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();

        _response = await _client.GetAsync("api/cities");
    }

    [Fact]
    public void Should_Be_Success()
    {
        Assert.True(_response.IsSuccessStatusCode);
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        _response.Dispose();
        await _webApplicationFactory.DisposeAsync();
    }
}