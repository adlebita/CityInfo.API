using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId:int}/[controller]")]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

        if (city is {NumberOfPointsOfInterest: > 0})
        {
            return Ok(city.PointsOfInterest);
        }

        return NotFound();
    }

    [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

        if (city is {NumberOfPointsOfInterest: > 0})
        {
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            return pointOfInterest != null ? Ok(pointOfInterest) : NotFound();
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId,
        [FromBody] PointOfInterestCreationDto pointOfInterestCreationDto)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
        if (city == null) return NotFound();

        var newId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

        var newPointOfInterest = new PointOfInterestDto
        {
            Id = ++newId,
            Name = pointOfInterestCreationDto.Name,
            Description = pointOfInterestCreationDto.Description
        };

        city.PointsOfInterest.Add(newPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest",
            new
            {
                cityId = city.Id,
                pointOfInterestId = newPointOfInterest.Id
            },
            newPointOfInterest);
    }
    
    [HttpPut("{pointOfInterestId:int}")]
    public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto updateDto)
    {
        var city = CitiesDataStore.Current.Cities.SingleOrDefault(x => x.Id == cityId);

        if (city == null)
            return NotFound();

        var pointOfInterest = city.PointsOfInterest.SingleOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest == null)
            return NotFound();

        pointOfInterest.Name = updateDto.Name;
        pointOfInterest.Description = updateDto.Description;
        
        return NoContent();
    }
    
    
    /**
     * This Patch endpoint uses the following packages for Patch protocols:
     * "Microsoft.AspNetCore.JsonPatch" Version="6.0.8"
     * "Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="6.0.8"
     */
    [HttpPatch("{pointOfInterestId:int}")]
    public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        var city = CitiesDataStore.Current.Cities.SingleOrDefault(x => x.Id == cityId);

        if (city == null)
            return NotFound();

        var pointOfInterest = city.PointsOfInterest.SingleOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest == null)
            return NotFound();

        var pointOfInterestToPatch = new PointOfInterestForUpdateDto
        {
            Description = pointOfInterest.Description,
            Name = pointOfInterest.Name
        };
        
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
        
        //This only checks the modelstate of the JSONPatchDocuemnt. Does not check model state of the object to patch.
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        //To validate the object that is being patched with new values, do below.
        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        pointOfInterest.Name = pointOfInterestToPatch.Name;
        pointOfInterest.Description = pointOfInterestToPatch.Description;
        
        return NoContent();
    }

    [HttpDelete("{pointOfInterestId:int}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.SingleOrDefault(x => x.Id == cityId);

        if (city == null)
            return NotFound();

        var pointOfInterest = city.PointsOfInterest.SingleOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest == null)
            return NotFound();

        city.PointsOfInterest.Remove(pointOfInterest);

        return NoContent();
    }
}