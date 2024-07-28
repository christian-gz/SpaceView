using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class DiameterMeasurement
{
    [JsonProperty("estimated_diameter_min")]
    public double Min { get; set; }

    [JsonProperty("estimated_diameter_max")]
    public double Max { get; set; }
}