namespace CityInfo.API.Models.Requests;

public sealed class CitiesFilterDto
{
    //Filter cities by Name
    public string? Name { get; set; } = string.Empty;
}