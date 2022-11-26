namespace CityInfo.API.Services;

public interface IUserRepository
{
    public Task<string?> AuthenticateUser(string email, string password);
}