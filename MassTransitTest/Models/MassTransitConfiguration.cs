namespace Models;
public class MassTransitConfiguration
{
    public const string MASS_TRANSIT_OPTION = "MassTransitCfg";

    public string RabbitMqHost { get; set; } = string.Empty;
    public string? RabbitUser { get; set; }
    public string? RabbitPass { get; set; }

}
