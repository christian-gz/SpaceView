using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class Neo
{
    [JsonProperty("id")]
    public string? ID { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("designation")]
    public string? Designation { get; set; }

    [JsonProperty("absolute_magnitude_h")]
    public string? Magnitude { get; set; }

    [JsonProperty("estimated_diameter")]
    public EstimatedDiameter EstimatedDiameter { get; set; } = new();

    [JsonProperty("is_potentially_hazardous_asteroid")]
    public bool IsPotentiallyHazardousAsteroid { get; set; }

    [JsonProperty("close_approach_data")]
    public List<CloseApproachData> CloseApproach { get; set; } = new();

    [JsonProperty("orbital_data")]
    public OrbitalData OrbitalData { get; set; } = new();

    [JsonProperty("is_sentry_object")]
    public bool IsSentryObject { get; set; }
}