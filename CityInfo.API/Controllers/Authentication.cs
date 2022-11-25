using CityInfo.API.Models.Requests;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Authentication : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public Authentication(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost("authenticate")]
    public async Task<ActionResult<string>> Authenticate(AuthenticateRequestBody request)
    {
        var isAuthenticated = await _userRepository.AuthenticateUser(request.Email, request.Password);
        return Ok();
    }
}