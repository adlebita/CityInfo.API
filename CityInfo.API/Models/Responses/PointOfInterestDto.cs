namespace CityInfo.API.Models.Responses;

public class PointOfInterestDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}