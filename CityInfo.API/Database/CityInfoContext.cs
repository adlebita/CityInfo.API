using CityInfo.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Database;

public class CityInfoContext : DbContext
{
    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; } = null!;

    public DbSet<PointOfInterest> PointsOfInterests { get; set; } = null!;
    
}