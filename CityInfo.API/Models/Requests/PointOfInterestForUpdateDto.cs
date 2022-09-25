namespace CityInfo.API.Models.Requests;

public class PointOfInterestForUpdateDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}