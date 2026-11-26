using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirportsApiService.Infrastructure.Data.Entities;
using AirlineCountrys.Dtos.Salida;
using AirlineCountryCreate.Dtos.Entrada;

namespace Airlines.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AirlinesController : ControllerBase
{
    private readonly AirportDbContext _context;

    public AirlinesController(AirportDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}/countries")]
    public async Task<IActionResult> GetCountriesByAirline(int id)
    {
        var airlineExists = await _context.Airlines.AnyAsync(a => a.Id == id);
        if (!airlineExists)
        {
            return NotFound(new { message = $"No existe la aerolínea con id {id}." });
        }

        var countries = await _context.AirlineCountries
            .Where(ac => ac.AirlineId == id)
            .Select(ac => new AirlineCountryDto
            {
                CountryId = ac.Country.Id,
                Code = ac.Country.Code,
                Name = ac.Country.Name,
                Native = ac.Native
            })
            .ToListAsync();

        return Ok(countries);
    }
    [HttpPost("{id:int}/countries")]
    public async Task<IActionResult> AddCountryToAirline(int id, [FromBody] AirlineCountryCreateDto dto)
    {
        var airlineExists = await _context.Airlines.AnyAsync(a => a.Id == id);
        if (!airlineExists)
        {
            return NotFound(new { message = $"No existe la aerolínea con id {id}." });
        }

        var countryExists = await _context.Countries.AnyAsync(c => c.Id == dto.CountryId);
        if (!countryExists)
        {
            return NotFound(new { message = $"No existe el país con id {dto.CountryId}." });
        }

        var alreadyAssociated = await _context.AirlineCountries
            .AnyAsync(ac => ac.AirlineId == id && ac.CountryId == dto.CountryId);
        if (alreadyAssociated)
        {
            return Conflict(new { message = "El país ya está asociado a esta aerolínea." });
        }

        var entity = new AirlineCountry
        {
            AirlineId = id,
            CountryId = dto.CountryId,
            Native = dto.Native
        };

        _context.AirlineCountries.Add(entity);
        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, entity);
}
}