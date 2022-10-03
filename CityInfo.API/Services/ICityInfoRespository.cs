using CityInfo.API.Models.Responses;

namespace CityInfo.API.Services;

public interface ICityInfoRespository
{
    public Task<CityDto?> GetCityById(Guid cityId);
    
    public Task<IEnumerable<CityDto>> GetCities();

    public Task<PointOfInterestDto?> GetPointOfInterestById(Guid pointOfInterestId);
    
    public Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterestByCityId(Guid cityId);
    
    public Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest();

    public Task<bool> DoesCityExist(Guid cityId);
}