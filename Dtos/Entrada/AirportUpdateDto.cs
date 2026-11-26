namespace AirportUpdate.Dtos.Entrada;

public class AirportUpdateDto
{
    public int CountryId { get; set; }
    public int? RegionId { get; set; }
    public string AirportCode { get; set; } = string.Empty;
    public byte Type { get; set; }
    public string? City { get; set; }
    public string? Municipality { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public short? ElevationFt { get; set; }
    public bool ScheduledService { get; set; }
    public string? GpsCode { get; set; }
    public string? Iatacode { get; set; }
    public string? LocalCode { get; set; }
    public string? HomeLink { get; set; }
    public string? WikipediaLink { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public double? FrequencyMhz { get; set; }
    public short? RunwayLengthFt { get; set; }
    public short? RunwayWidthFt { get; set; }
    public string? RunwaySurface { get; set; }
    public bool RunwayLighted { get; set; }
    public bool RunwayClosed { get; set; }
}