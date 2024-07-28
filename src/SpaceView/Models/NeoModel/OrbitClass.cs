using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class OrbitClass
{
    [JsonProperty("orbit_class_type")]
    public string? OrbitClassType { get; set; }

    [JsonProperty("orbit_class_description")]
    public string? OrbitClassDescription { get; set; }

    [JsonProperty("orbit_class_range")]
    public string? OrbitClassRange { get; set; }
}