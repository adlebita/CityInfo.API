using CityInfo.API.Models;
using CityInfo.API.Models.Responses;

namespace CityInfo.API;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    
    public CitiesDataStore()
    {
        Cities = new List<CityDto>()
        {
            new CityDto
            {
                Id = 1,
                Name = "New York",
                Description = "Big apple",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 1,
                        Name = "Statue of Liberty",
                        Description = "Green statue!"
                    }
                }
            },
            new CityDto
            {
                Id = 2,
                Name = "Tokyo",
                Description = "Mega city",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 1,
                        Name = "Tokyo Sky Tree",
                        Description = "Very tall!"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 2,
                        Name = "Shibuya Crossing",
                        Description = "So many people!"
                    }
                }
            },
            new CityDto()
            {
                Id = 3,
                Name = "Wellington",
                Description = "Alternative life",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 1,
                        Name = "Brooklyn Windmill",
                        Description = "Super windy!"
                    }
                }
            }
        };
    }
}