using CityInfo.API.Models.Requests;
using CityInfo.API.Models.Requests.Authentication;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthenticationController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost("authenticate")]
    public async Task<ActionResult<string>> Authenticate(AuthenticateRequestBody request)
    {
        var jwtToken = await _userRepository.AuthenticateUser(request.Email, request.Password);

        return jwtToken != null ? Ok(jwtToken) : Unauthorized();
    }
}