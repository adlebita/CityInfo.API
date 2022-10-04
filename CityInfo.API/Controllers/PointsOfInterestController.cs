using CityInfo.API.Models.Requests;
using CityInfo.API.Models.Responses;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId:guid}/[controller]")]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRespository _cityInfoRespository;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
        ICityInfoRespository cityInfoRespository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        _cityInfoRespository = cityInfoRespository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterestByCityId(Guid cityId)
    {
        var cityExists = await _cityInfoRespository.DoesCityExist(cityId);

        if (cityExists == false)
        {
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

    // [HttpPost]
    // public ActionResult<PointOfInterestDto> CreatePointOfInterest(Guid cityId,
    //     [FromBody] PointOfInterestCreationDto pointOfInterestCreationDto)
    // {
    //     var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
    //     if (city == null) return NotFound();
    //
    //     var newId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
    //
    //     var newPointOfInterest = new PointOfInterestDto
    //     {
    //         Id = ++newId,
    //         Name = pointOfInterestCreationDto.Name,
    //         Description = pointOfInterestCreationDto.Description
    //     };
    //
    //     city.PointsOfInterest.Add(newPointOfInterest);
    //
    //     return CreatedAtRoute("GetPointOfInterest",
    //         new
    //         {
    //             cityId = city.Id,
    //             pointOfInterestId = newPointOfInterest.Id
    //         },
    //         newPointOfInterest);
    // }
    //
    // [HttpPut("{pointOfInterestId:int}")]
    // public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto updateDto)
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
    //     pointOfInterest.Name = updateDto.Name;
    //     pointOfInterest.Description = updateDto.Description;
    //
    //     return NoContent();
    // }
    //
    //
    // /**
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
    // [HttpDelete("{pointOfInterestId:int}")]
    // public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
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
    //     city.PointsOfInterest.Remove(pointOfInterest);
    //     _mailService.Send("PoI Deleted", $"Point of Interest Id: {pointOfInterestId} was deleted from {city.Name}");
    //
    //     return NoContent();
    // }
}