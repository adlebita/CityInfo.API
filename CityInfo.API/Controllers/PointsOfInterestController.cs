using CityInfo.API.Models.Requests;
using CityInfo.API.Models.Requests.PointsOfInterest;
using CityInfo.API.Models.Responses;
using CityInfo.API.Models.Responses.PointsOfInterest;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Authorize]
[Route("api/cities/{cityId:guid}/[controller]")]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly ICityInfoRespository _cityInfoRespository;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, ICityInfoRespository cityInfoRespository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cityInfoRespository = cityInfoRespository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterestByCityId(Guid cityId)
    {
        var cityExists = await _cityInfoRespository.DoesCityExist(cityId);

        if (cityExists == false)
        {
            //Todo: Implement proper pipeline logging with SeriLog
            _logger.LogError($"City '{cityId}' could not be found."); 
            return NotFound();
        }

        var pois = await _cityInfoRespository.GetPointsOfInterestByCityId(cityId);
        return Ok(pois);
    }

    [HttpGet("{pointOfInterestId:Guid}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterestById(Guid _, Guid pointOfInterestId)
    {
        var pointOfInterest = await _cityInfoRespository.GetPointOfInterestById(pointOfInterestId);

        return pointOfInterest != null ? Ok(pointOfInterest) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(Guid cityId,
        [FromBody] CreatePointOfInterestDto createPointOfInterestDto)
    {
        var cityExists = await _cityInfoRespository.GetCityById(cityId);

        if (cityExists == null) return NotFound();

        var newPoI =
            await _cityInfoRespository.CreateNewPointOfInterest(cityId, createPointOfInterestDto);

        return CreatedAtRoute("GetPointOfInterest", new {cityId, pointOfInterestId = newPoI.Id}, newPoI);
    }
    
    [HttpPut("{pointOfInterestId:Guid}")]
    public async Task<ActionResult> UpdatePointOfInterest(UpdatePointOfInterestDto updatePointOfInterestDto, Guid cityId, Guid pointOfInterestId)
    {
        var doesPoIExist = await _cityInfoRespository.DoesPointOfInterestExist(pointOfInterestId);

        if (doesPoIExist == false) return NotFound();
        
        await _cityInfoRespository.UpdatePointOfInterest(pointOfInterestId, updatePointOfInterestDto);
    
        return NoContent();
    }
    
    [HttpDelete("{pointOfInterestId:Guid}")]
    public async Task<ActionResult> DeletePointOfInterest(Guid cityId, Guid pointOfInterestId)
    {
        var doesPoIExist = await _cityInfoRespository.DoesPointOfInterestExist(pointOfInterestId);

        if (doesPoIExist == false) return NotFound();

        await _cityInfoRespository.DeletePointOfInterest(pointOfInterestId);
        
        return NoContent();
    }

    [HttpPatch("{pointOfInterestId:Guid}")]
    public async Task<ActionResult> UpdatePointOfInterestDescription(Guid pointOfInterestId, UpdatePointOfInterestDescriptionDto updatePointOfInterestDescriptionDto)
    {
        var doesPoIExist = await _cityInfoRespository.DoesPointOfInterestExist(pointOfInterestId);

        if (doesPoIExist == false) return NotFound();

        await _cityInfoRespository.UpdatePointOfInterestDescription(pointOfInterestId, updatePointOfInterestDescriptionDto);

        return NoContent();
    }

    // /**
    //  * 12/10/2022 - I don't like this method of patching. See UpdatePoIDescription action method instead for implementation
    //  * of HTTP Patch. Have left this in for reference. Consider using PATCH, if necessary and ideally for entities with 
    //  * properties that can be patched... but for this project is too cumbersome. Remember, using PUT instead of PATCH while no
    //  * no one will pull up for it, it's not in line with REST API architecture.
    //  * https://stackoverflow.com/questions/19732423/why-isnt-http-put-allowed-to-do-partial-updates-in-a-rest-api
    //  *
    //  * This Patch endpoint uses the following packages for Patch protocols:
    //  * "Microsoft.AspNetCore.JsonPatch" Version="6.0.8"
    //  * "Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="6.0.8"
    //  */
    // [HttpPatch("{pointOfInterestId:int}")]
    // public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
    //     JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    // {
    //     var city = _citiesDataStore.Cities.SingleOrDefault(x => x.Id == cityId);
    //
    //     if (city == null)
    //         return NotFound();
    //
    //     var pointOfInterest = city.PointsOfInterest.SingleOrDefault(x => x.Id == pointOfInterestId);
    //
    //     if (pointOfInterest == null)
    //         return NotFound();
    //
    //     var pointOfInterestToPatch = new PointOfInterestForUpdateDto
    //     {
    //         Description = pointOfInterest.Description,
    //         Name = pointOfInterest.Name
    //     };
    //
    //     patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
    //
    //     //This only checks the modelstate of the JSONPatchDocuemnt. Does not check model state of the object to patch.
    //     if (!ModelState.IsValid)
    //         return BadRequest(ModelState);
    //
    //     //To validate the object that is being patched with new values, do below.
    //     if (!TryValidateModel(pointOfInterestToPatch))
    //     {
    //         return BadRequest(ModelState);
    //     }
    //
    //     pointOfInterest.Name = pointOfInterestToPatch.Name;
    //     pointOfInterest.Description = pointOfInterestToPatch.Description;
    //
    //     return NoContent();
    // }
    //
    
}