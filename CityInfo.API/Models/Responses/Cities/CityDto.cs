using CityInfo.API.Models.Responses.PointsOfInterest;

namespace CityInfo.API.Models.Responses.Cities;

public sealed record CityDto(Guid Id, string Name, IEnumerable<PointOfInterestDto> PointsOfInterest)
{
    public string? Description { get; set; }

    public int NumberOfPointsOfInterest => PointsOfInterest.Count();
}