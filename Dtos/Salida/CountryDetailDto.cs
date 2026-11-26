namespace CountryDetail.Dtos.Salida;

public class CountryDetailDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Continent { get; set; }
    public string? Keywords { get; set; }
    public string? WikipediaLink { get; set; }
    public List<RegionSummaryDto> Regions { get; set; } = new();
    public List<AirportTypeCountDto> AirportsByType { get; set; } = new();
    public List<NativeAirlineDto> NativeAirlines { get; set; } = new();
}

public class RegionSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string LocalCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class AirportTypeCountDto
{
    public byte Type { get; set; }
    public int Count { get; set; }
}

public class NativeAirlineDto
{
    public int AirlineId { get; set; }
    public string Name { get; set; } = string.Empty;
}