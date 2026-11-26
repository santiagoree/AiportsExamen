using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirportsApiService.Infrastructure.Data.Entities;
using CountryCreate.Dtos.Entrada;
using CountryDetail.Dtos.Salida;

namespace Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly AirportDbContext _context;

    public CountriesController(AirportDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCountryDetail(int id)
    {
        var country = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country is null)
        {
            return NotFound(new { message = $"No existe el país con id {id}." });
        }

        var regions = await _context.Regions
            .AsNoTracking()
            .Where(r => r.CountryId == id)
            .Select(r => new RegionSummaryDto
            {
                Id = r.Id,
                Code = r.Code,
                LocalCode = r.LocalCode,
                Name = r.Name
            })
            .ToListAsync();

        var airportsByType = await _context.Airports
            .AsNoTracking()
            .Where(a => a.CountryId == id)
            .GroupBy(a => a.Type)
            .Select(g => new AirportTypeCountDto
            {
                Type = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var nativeAirlines = await _context.AirlineCountries
            .AsNoTracking()
            .Where(ac => ac.CountryId == id && ac.Native)
            .Select(ac => new NativeAirlineDto
            {
                AirlineId = ac.AirlineId,
                Name = ac.Airline.Name
            })
            .ToListAsync();

        var result = new CountryDetailDto
        {
            Id = country.Id,
            Code = country.Code,
            Name = country.Name,
            Continent = country.Continent,
            Keywords = country.Keywords,
            WikipediaLink = country.WikipediaLink,
            Regions = regions,
            AirportsByType = airportsByType,
            NativeAirlines = nativeAirlines
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCountry([FromBody] CountryCreateDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var country = new Country
            {
                Code = dto.Code,
                Name = dto.Name,
                Continent = dto.Continent,
                Keywords = dto.Keywords,
                WikipediaLink = dto.WikipediaLink
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            if (dto.Regions is { Count: > 0 })
            {
                foreach (var regionDto in dto.Regions)
                {
                    var region = new Region
                    {
                        CountryId = country.Id,
                        Code = regionDto.Code,
                        LocalCode = regionDto.LocalCode,
                        Name = regionDto.Name,
                        Keywords = regionDto.Keywords,
                        WikipediaLink = regionDto.WikipediaLink
                    };
                    _context.Regions.Add(region);
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return StatusCode(StatusCodes.Status201Created, new
            {
                country.Id,
                country.Code,
                country.Name,
                country.Continent
            });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}