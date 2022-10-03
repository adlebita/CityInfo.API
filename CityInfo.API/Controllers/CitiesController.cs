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
        return Ok(await _cityInfoRespository.GetCities());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> GetCity(Guid id)
    {
        var city = await _cityInfoRespository.GetCityById(id);
        
        if (city == null)
        {
            return NotFound();
        }

        return Ok(city);
    }
}