using CityInfo.API.Models.Requests;
using CityInfo.API.Models.Responses;

namespace CityInfo.API.Services;

public interface ICityInfoRespository
{
    public Task<CityDto?> GetCityById(Guid cityId);
    
    public Task<IEnumerable<CityDto>> GetCities();
    
    public Task<IEnumerable<CityDto>> GetCitiesWithFilter(CitiesFilterDto citiesFilter);

    public Task<PointOfInterestDto?> GetPointOfInterestById(Guid pointOfInterestId);
    
    public Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterestByCityId(Guid cityId);
    
    public Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest();

    public Task<PointOfInterestDto> CreateNewPointOfInterest(Guid cityId, CreatePointOfInterestDto createPointOfInterestDto);

    public Task UpdatePointOfInterest(UpdatePointOfInterestDto updatePointOfInterestDto);

    public Task<bool> DoesCityExist(Guid cityId);

    public Task<bool> DoesPointOfInterestExist(Guid poiId);
}