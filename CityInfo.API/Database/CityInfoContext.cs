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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(
                new City
                {
                    Id = Guid.Parse("8C9B6082-C6FC-4E0F-99D6-904B880BBA33"),
                    Name = "New York",
                    Description = "Big apple"
                },
                new City
                {
                    Id = Guid.Parse("FC1C76C4-3E26-4E90-9A07-35D42C9C2D74"),
                    Name = "Tokyo",
                    Description = "Mega city"
                },
                new City
                {
                    Id = Guid.Parse("03A051AC-484B-47C2-BDAC-AF461F4122FF"),
                    Name = "Wellington",
                    Description = "Alternative life"
                }
            );

        modelBuilder.Entity<PointOfInterest>()
            .HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    CityId = Guid.Parse("8C9B6082-C6FC-4E0F-99D6-904B880BBA33"),
                    Name = "Statue of Liberty",
                    Description = "Green statue!"
                },
                new
                {
                    Id = Guid.NewGuid(),
                    CityId = Guid.Parse("FC1C76C4-3E26-4E90-9A07-35D42C9C2D74"),
                    Name = "Tokyo Sky Tree",
                    Description = "Very tall!"
                },
                new
                {
                    Id = Guid.NewGuid(),
                    CityId = Guid.Parse("FC1C76C4-3E26-4E90-9A07-35D42C9C2D74"),
                    Name = "Shibuya Crossing",
                    Description = "So many people!"
                },
                new
                {
                    Id = Guid.NewGuid(),
                    CityId = Guid.Parse("03A051AC-484B-47C2-BDAC-AF461F4122FF"),
                    Name = "Brooklyn Windmill",
                    Description = "Super windy!"
                }
            );

        base.OnModelCreating(modelBuilder);
    }
}