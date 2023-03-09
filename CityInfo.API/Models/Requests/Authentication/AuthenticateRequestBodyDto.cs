namespace CityInfo.API.Models.Requests.Authentication;

public sealed class AuthenticateRequestBody
{
    public string Email { get; set; }
    public string Password { get; set; }
}