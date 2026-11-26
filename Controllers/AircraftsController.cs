using Microsoft.AspNetCore.Mvc;
using AirportsApiService.Infrastructure.Data.Entities;
using Aircrafts.Dtos.Entrada;

namespace Aircrafts.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AircraftsController : ControllerBase
{
    private readonly AirportDbContext _context;

    public AircraftsController(AirportDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AircraftsDto dto)
    {
        var entity = new Aircraft
        {
            Iatacode = dto.IataCode,
            Name = dto.Name
        };

        _context.Add(entity);
        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, entity);
    }
}