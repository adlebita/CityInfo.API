using CityInfo.API.Database;
using CityInfo.API.Models.Entity;
using CityInfo.API.Models.Requests;
using CityInfo.API.Models.Requests.Cities;
using CityInfo.API.Models.Requests.PointsOfInterest;
using CityInfo.API.Models.Responses;
using CityInfo.API.Models.Responses.Cities;
using CityInfo.API.Models.Responses.PointsOfInterest;
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

        var pointOfInterestDtos = city.PointsOfInterest
            .Select(poi => new PointOfInterestDto(poi.Id, poi.Name) {Description = poi.Description});

        return new CityDto(city.Id, city.Name, pointOfInterestDtos)
        {
            Description = city.Description
        };
    }

    public async Task<IEnumerable<CityDto>> GetCities(int pageNumber, int pageSize)
    {
        var cities = await _db.Cities.Include(c => c.PointsOfInterest)
            .OrderBy(c => c.Name)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return cities.Count == 0
            ? Enumerable.Empty<CityDto>()
            : cities.Select(city =>
            {
                var poi = city.PointsOfInterest.Select(poi => new PointOfInterestDto(poi.Id, poi.Name)
                    {Description = poi.Description});
                return new CityDto(city.Id, city.Name, poi) {Description = city.Description};
            });
    }

    public async Task<IEnumerable<CityDto>> GetCitiesWithFilter(CitiesFilterDto citiesFilter, int pageNumber, int pageSize)
    {
        var cityName = citiesFilter.Name;

        IEnumerable<City> cities;

        if (string.IsNullOrEmpty(cityName) != true)
        {
            cities = _db.Cities.Include(c => c.PointsOfInterest)
                .Where(c => c.Name == cityName.Trim() || 
                            c.Description != null && c.Description.Contains(cityName.Trim()))
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsEnumerable();
        }
        else
        {
            cities = _db.Cities.Include(c => c.PointsOfInterest)
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsEnumerable();
        }
        
        var filteredCities = new List<CityDto>();
        foreach (var city in cities)
        {
            var poi = city.PointsOfInterest
                .Select(poi => new PointOfInterestDto(poi.Id, poi.Name) {Description = poi.Description});

            filteredCities.Add(new CityDto(city.Id, city.Name, poi) {Description = city.Description});
        }

        return filteredCities;
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

    public async Task<PointOfInterestDto> CreateNewPointOfInterest(Guid cityId,
        CreatePointOfInterestDto createPointOfInterestDto)
    {
        var newPointOfInterest = new PointOfInterest
        {
            Name = createPointOfInterestDto.Name,
            Description = createPointOfInterestDto.Description,
            CityId = cityId
        };

        await _db.PointsOfInterests.AddAsync(newPointOfInterest);
        await _db.SaveChangesAsync();

        return new PointOfInterestDto(newPointOfInterest.Id, newPointOfInterest.Name)
            {Description = newPointOfInterest.Description};
    }

    public async Task UpdatePointOfInterest(Guid pointOfInterestId, UpdatePointOfInterestDto updatePointOfInterestDto)
    {
        var existingPoi = await
            _db.PointsOfInterests
                .SingleOrDefaultAsync(poi => poi.Id == pointOfInterestId);

        ArgumentNullException.ThrowIfNull(existingPoi, nameof(existingPoi.Id));

        existingPoi.Name = updatePointOfInterestDto.Name;
        existingPoi.Description = updatePointOfInterestDto.Description;

        await _db.SaveChangesAsync();
    }

    public async Task DeletePointOfInterest(Guid pointOfInterestId)
    {
        var poi = await _db.PointsOfInterests.SingleOrDefaultAsync(poi => poi.Id == pointOfInterestId);
        
        ArgumentNullException.ThrowIfNull(poi, nameof(poi.Id));

        _db.PointsOfInterests.Remove(poi);
        await _db.SaveChangesAsync();
    }

    public async Task UpdatePointOfInterestDescription(Guid pointOfInterestId,
        UpdatePointOfInterestDescriptionDto updatePointOfInterestDescriptionDto)
    {
        var poi = await _db.PointsOfInterests.SingleOrDefaultAsync(poi => poi.Id == pointOfInterestId);
        
        ArgumentNullException.ThrowIfNull(poi, nameof(poi.Id));

        poi.Description = updatePointOfInterestDescriptionDto.Description;
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DoesPointOfInterestExist(Guid poiId)
    {
        return await _db.PointsOfInterests.AnyAsync(poi => poi.Id == poiId);
    }

    public async Task<bool> DoesCityExist(Guid cityId)
    {
        return await _db.Cities.AnyAsync(c => c.Id == cityId);
    }
}