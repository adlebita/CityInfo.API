using System.Text;
using CityInfo.API.Database;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, _, configuration) =>
{
    configuration.WriteTo.Console();
    configuration.WriteTo.File("Logs/cityinfo.txt", rollingInterval: RollingInterval.Day);
});

// Add services to the container.
builder.Services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICityInfoRespository, CityInfoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

/*
 * Connection string: https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/creating-a-connection-string
 */
builder.Services.AddDbContext<CityInfoContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CityInfoContext");
    options.UseSqlServer(connectionString);
});

builder.Services.AddDbContext<UserInfoContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("UserContext");
    options.UseSqlServer(connectionString);
});

//Adding authentication scheme. Here we use AddJwtBearer, out of the box comes loaded.
//We configure the scheme with the JWT options. In this case, when a token comes in:
//We validate, the issuer of the token, audience and signing key. When adding fields to be validated, the scheme needs
//to know what to validate against... we specify this with ValidIssuer, ValidAudience and IssuerSigningKey.
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    });

builder.Services.AddAuthorization(options =>
    options.AddPolicy("UserMustBeAHolmes", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.AddRequirements(new AssertionRequirement(context =>
        {
            var userName =
                context.User.Claims.First(c => c.Type.Contains("Name", StringComparison.InvariantCultureIgnoreCase));
            return userName.Value.Contains("holmes", StringComparison.InvariantCultureIgnoreCase);
        }));
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//  This requires the builder.Services.AddControllers() invoked.
//  This method allows, controller classes to be found/used without specifying.
app.MapControllers();

app.Run();

namespace CityInfo.API
{
    public partial class Program { }
}