namespace CityInfo.API.Models.Requests;

public class UpdatePointOfInterestDto
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}