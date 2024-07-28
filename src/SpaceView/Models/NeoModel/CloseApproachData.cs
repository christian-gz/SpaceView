using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class CloseApproachData
{
    [JsonProperty("close_approach_date_full")]
    public string? CloseApproachDate { get; set; }

    [JsonProperty("relative_velocity")]
    public Velocity? RelativeVelocity { get; set; }

    [JsonProperty("miss_distance")]
    public Distance? MissDistance { get; set; }
}