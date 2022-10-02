using CityInfo.API.Models.Responses;

namespace CityInfo.API.Services;

public interface ICityInfoRespository
{
    public Task<CityDto?> GetCityByIdAsync(Guid cityId);
    
    public Task<IEnumerable<CityDto>> GetCitiesAsync();
}