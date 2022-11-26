using CityInfo.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Database;

public class UserInfoContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public UserInfoContext(DbContextOptions<UserInfoContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Mycroft Holmes",
                    Email = "mycrofth@britain.uk",
                    Password = "123456"
                });
        
        base.OnModelCreating(modelBuilder);
    }
}