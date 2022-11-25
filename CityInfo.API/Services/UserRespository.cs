using CityInfo.API.Database;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public sealed class UserRepository : IUserRepository
{
    private readonly UserInfoContext _userInfoContext;

    public UserRepository(UserInfoContext userInfoContext)
    {
        _userInfoContext = userInfoContext ?? throw new ArgumentNullException($"Database {nameof(UserInfoContext)} is null.");
    }

    public async Task<bool> AuthenticateUser(string email, string password)
    {
        var user = await _userInfoContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            return password == user.Password;
        }

        return false;
    }
}