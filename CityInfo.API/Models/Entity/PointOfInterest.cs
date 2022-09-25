using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Models.Entity;

public class PointOfInterest
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [ForeignKey("CityId")]
    public Guid CityId {get; set;}
    
    [MaxLength(200)]
    public string? Description { get; set; }
}