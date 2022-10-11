namespace CityInfo.API.Models.Requests;

public class UpdatePointOfInterestDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}