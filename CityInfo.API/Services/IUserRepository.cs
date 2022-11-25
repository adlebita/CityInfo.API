namespace CityInfo.API.Services;

public interface IUserRepository
{
    public Task<bool> AuthenticateUser(string email, string password);
}