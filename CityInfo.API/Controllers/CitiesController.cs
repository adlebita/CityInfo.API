using CityInfo.API.Models.Responses;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRespository _cityInfoRespository;

    public CitiesController(ICityInfoRespository cityInfoRespository)
    {
        _cityInfoRespository = cityInfoRespository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
    {
        return Ok(await _cityInfoRespository.GetCitiesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> GetCity(string id)
    {
        var city = await _cityInfoRespository.GetCityByIdAsync(Guid.Parse(id));
        
        if (city == null)
        {
            return NotFound(city);
        }

        return Ok(city);
    }
}