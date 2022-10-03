using CityInfo.API.Database;
using CityInfo.API.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public sealed class CityInfoRepository : ICityInfoRespository
{
    private readonly CityInfoContext _db;

    public CityInfoRepository(CityInfoContext db)
    {
        _db = db ?? throw new ArgumentNullException($"Database {nameof(CityInfoContext)} is null.");
    }

    public async Task<CityDto?> GetCityById(Guid cityId)
    {
        var city = await _db.Cities.Include(c => c.PointsOfInterest).SingleOrDefaultAsync(c => c.Id == cityId);

        if (city == null) return null;

        var pointOfInterestDtos = city.PointsOfInterest.Select(poi => new PointOfInterestDto(poi.Id, poi.Name){ Description = poi.Description});

        return new CityDto(city.Id, city.Name, pointOfInterestDtos)
        {
            Description = city.Description
        };
    }

    public async Task<IEnumerable<CityDto>> GetCities()
    {
        var cities = await _db.Cities.Include(c => c.PointsOfInterest).OrderBy(c => c.Name).ToListAsync();

        return cities.Count == 0
            ? Enumerable.Empty<CityDto>()
            : cities.Select(city =>
            {
                var poi = city.PointsOfInterest.Select(poi => new PointOfInterestDto(poi.Id, poi.Name)
                    {Description = poi.Description});
                return new CityDto(city.Id, city.Name, poi) {Description = city.Description};
            });
    }

    public async Task<PointOfInterestDto?> GetPointOfInterestById(Guid pointOfInterestId)
    {
        var poi = await _db.PointsOfInterests.FirstOrDefaultAsync(poi => poi.Id == pointOfInterestId);

        return poi != null
            ? new PointOfInterestDto(poi.Id, poi.Name) {Description = poi.Description}
            : null;
    }

    public async Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterestByCityId(Guid cityId)
    {
        var pois = await _db.PointsOfInterests.Where(poi => poi.CityId == cityId).ToListAsync();
        
        return pois.Count == 0
            ? Enumerable.Empty<PointOfInterestDto>()
            : pois.Select(poi => new PointOfInterestDto(poi.Id, poi.Name) {Description = poi.Description});
    }

    public async Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest()
    {
        var pois = await _db.PointsOfInterests.ToListAsync();

        return pois.Count == 0
            ? Enumerable.Empty<PointOfInterestDto>()
            : pois.Select(poi => new PointOfInterestDto(poi.Id, poi.Name) {Description = poi.Description});
    }

    public async Task<bool> DoesCityExist(Guid cityId)
    {
        return await _db.Cities.FirstOrDefaultAsync(c => c.Id == cityId) != null;
    }
}