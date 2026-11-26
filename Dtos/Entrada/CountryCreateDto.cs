namespace CountryCreate.Dtos.Entrada;

public class CountryCreateDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Continent { get; set; }
    public string? Keywords { get; set; }
    public string? WikipediaLink { get; set; }
    public List<RegionCreateItemDto>? Regions { get; set; }
}

public class RegionCreateItemDto
{
    public string Code { get; set; } = string.Empty;
    public string LocalCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Keywords { get; set; }
    public string? WikipediaLink { get; set; }
}