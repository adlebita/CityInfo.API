using CityInfo.API.Models.Requests.Cities;
using CityInfo.API.Models.Requests.PointsOfInterest;
using CityInfo.API.Models.Responses.Cities;
using CityInfo.API.Models.Responses.PointsOfInterest;

namespace CityInfo.API.Services;

public interface ICityInfoRespository
{
    public Task<CityDto?> GetCityById(Guid cityId);
    
    public Task<IEnumerable<CityDto>> GetCities(int pageNumber, int pageSize);
    
    public Task<IEnumerable<CityDto>> GetCitiesWithFilter(CitiesFilterDto citiesFilter, int pageNumber, int pageSize);

    public Task<PointOfInterestDto?> GetPointOfInterestById(Guid pointOfInterestId);
    
    public Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterestByCityId(Guid cityId);
    
    public Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest();

    public Task<PointOfInterestDto> CreateNewPointOfInterest(Guid cityId, CreatePointOfInterestDto createPointOfInterestDto);

    public Task UpdatePointOfInterest(Guid pointOfInterestId, UpdatePointOfInterestDto updatePointOfInterestDto);

    public Task DeletePointOfInterest(Guid pointOfInterestId);
    
    Task UpdatePointOfInterestDescription(Guid pointOfInterestId, UpdatePointOfInterestDescriptionDto updatePointOfInterestDescriptionDto);

    public Task<bool> DoesCityExist(Guid cityId);

    public Task<bool> DoesPointOfInterestExist(Guid poiId);
}