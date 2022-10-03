using System.Collections.Concurrent;

namespace CityInfo.API.Models.Responses;

public sealed record CityDto(Guid Id, string Name, IEnumerable<PointOfInterestDto> PointsOfInterest)
{
    public string? Description { get; set; }

    public int NumberOfPointsOfInterest => PointsOfInterest.Count();
}