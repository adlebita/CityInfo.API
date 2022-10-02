using CityInfo.API.Database;
using CityInfo.API.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public sealed class CityInfoRepository : ICityInfoRespository
{
    private readonly CityInfoContext _db;

    public CityInfoRepository(CityInfoContext db)
    {
        _db = db ?? throw new ArgumentNullException($"No {nameof(CityInfoContext)} is null.");
    }

    public async Task<CityDto?> GetCityByIdAsync(Guid cityId)
    {
        var city = await _db.Cities.Include(c => c.PointsOfInterest).SingleOrDefaultAsync(c => c.Id == cityId);

        if (city == null) return null;
        
        var pointOfInterestDtos = city.PointsOfInterest.Select(poi => new PointOfInterestDto
        {
            Id = poi.Id, Name = poi.Name, Description = poi.Description
        }).ToList();

        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Description = city.Description ?? null,
            PointsOfInterest = pointOfInterestDtos
        };
    }

    public async Task<IEnumerable<CityDto>> GetCitiesAsync()
    {
        var cities = await _db.Cities.Include(c => c.PointsOfInterest).ToListAsync();

        var cityDto = cities.Select(city =>
            new CityDto
            {
                Id = city.Id, Name = city.Name, Description = city.Description,
                PointsOfInterest = city.PointsOfInterest.Select(poi => new PointOfInterestDto
                {
                    Id = poi.Id, Name = poi.Name, Description = poi.Description
                }).ToList()
            }).ToList();

        return cityDto;
    }
}