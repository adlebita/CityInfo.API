using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CityInfo.API.Database;
using CityInfo.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Services;

public sealed class UserRepository : IUserRepository
{
    private readonly UserInfoContext _userInfoContext;
    private readonly IConfiguration _configuration;

    public UserRepository(UserInfoContext userInfoContext, IConfiguration configuration)
    {
        _userInfoContext = userInfoContext ??
                           throw new ArgumentNullException($"Database {nameof(UserInfoContext)} is null.");
        _configuration = configuration ??
                         throw new ArgumentNullException($"Configuration {nameof(configuration)} is null.");
    }

    public async Task<string?> AuthenticateUser(string email, string password)
    {
        var user = await _userInfoContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            if (password == user.Password)
            {
                return GenerateJwtToken(user);
            }
        }

        return null;
    }

    private string? GenerateJwtToken(User user)
    {
        var securityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new("id", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email)
        };

        var token = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            signingCredentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return jwtToken;
    }
}