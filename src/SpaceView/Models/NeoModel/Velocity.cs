using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class Velocity
{
    [JsonProperty("kilometers_per_second")]
    public double KilometersPerSecond { get; set; }

    [JsonProperty("kilometers_per_hour")]
    public double KilometersPerHour { get; set; }
}