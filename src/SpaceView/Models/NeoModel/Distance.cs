using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class Distance
{
    [JsonProperty("astronomical")]
    public double Astronomical { get; set; }

    [JsonProperty("lunar")]
    public double Lunar { get; set; }

    [JsonProperty("kilometers")]
    public double Kilometers { get; set; }
}