using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirportsApiService.Infrastructure.Data.Entities;
using AirportUpdate.Dtos.Entrada;

namespace Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AirportsController : ControllerBase
{
    private readonly AirportDbContext _context;

    public AirportsController(AirportDbContext context)
    {
        _context = context;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAirport(int id, [FromBody] AirportUpdateDto dto)
    {
        var airport = await _context.Airports.FirstOrDefaultAsync(a => a.Id == id);
        if (airport is null)
        {
            return NotFound(new { message = $"No existe el aeropuerto con id {id}." });
        }

        var countryExists = await _context.Countries.AnyAsync(c => c.Id == dto.CountryId);
        if (!countryExists)
        {
            return NotFound(new { message = $"No existe el país con id {dto.CountryId}." });
        }

        if (dto.RegionId.HasValue)
        {
            var regionExists = await _context.Regions.AnyAsync(r => r.Id == dto.RegionId.Value);
            if (!regionExists)
            {
                return NotFound(new { message = $"No existe la región con id {dto.RegionId.Value}." });
            }
        }

        airport.CountryId = dto.CountryId;
        airport.RegionId = dto.RegionId;
        airport.AirportCode = dto.AirportCode;
        airport.Type = dto.Type;
        airport.City = dto.City;
        airport.Municipality = dto.Municipality;
        airport.Name = dto.Name;
        airport.Latitude = dto.Latitude;
        airport.Longitude = dto.Longitude;
        airport.ElevationFt = dto.ElevationFt;
        airport.ScheduledService = dto.ScheduledService;
        airport.GpsCode = dto.GpsCode;
        airport.Iatacode = dto.Iatacode;
        airport.LocalCode = dto.LocalCode;
        airport.HomeLink = dto.HomeLink;
        airport.WikipediaLink = dto.WikipediaLink;
        airport.Keywords = dto.Keywords;
        airport.Description = dto.Description;
        airport.FrequencyMhz = dto.FrequencyMhz;
        airport.RunwayLengthFt = dto.RunwayLengthFt;
        airport.RunwayWidthFt = dto.RunwayWidthFt;
        airport.RunwaySurface = dto.RunwaySurface;
        airport.RunwayLighted = dto.RunwayLighted;
        airport.RunwayClosed = dto.RunwayClosed;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}