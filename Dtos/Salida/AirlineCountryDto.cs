namespace AirlineCountrys.Dtos.Salida;

public class AirlineCountryDto
{
    public int CountryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Native { get; set; }
}