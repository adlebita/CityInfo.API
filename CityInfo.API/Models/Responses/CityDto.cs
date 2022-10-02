namespace CityInfo.API.Models.Responses;

public class CityDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IEnumerable<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

    public int NumberOfPointsOfInterest => PointsOfInterest.Count();
}