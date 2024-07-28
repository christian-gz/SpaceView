using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class EstimatedDiameter
{
    [JsonProperty("kilometers")]
    public DiameterMeasurement? Kilometers { get; set; }

    [JsonProperty("meters")]
    public DiameterMeasurement Meters { get; set; } = new();

    [JsonProperty("miles")]
    public DiameterMeasurement? Miles { get; set; }

    [JsonProperty("feet")]
    public DiameterMeasurement? Feet { get; set; }
}