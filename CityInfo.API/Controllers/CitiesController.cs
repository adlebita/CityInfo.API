using CityInfo.API.Models.Requests;
using CityInfo.API.Models.Requests.Cities;
using CityInfo.API.Models.Responses;
using CityInfo.API.Models.Responses.Cities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCities([FromQuery] CitiesFilterDto citiesFilter, 
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1)
    {
        return Ok(await _cityInfoRespository.GetCitiesWithFilter(citiesFilter, pageNumber, pageSize));
    }
    
    [Authorize(Policy = "UserMustBeAHolmes")]
    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> GetCity(Guid id)
    {
        var city = await _cityInfoRespository.GetCityById(id);
        
        if (city == null) return NotFound();

        return Ok(city);
    }
}