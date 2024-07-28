using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class OrbitalData
{
    [JsonProperty("orbit_id")]
    public string? ID { get; set; }

    [JsonProperty("first_observation_date")]
    public string? FirstObservationDate { get; set; }

    [JsonProperty("orbit_uncertainty")]
    public string? OrbitUncertainty { get; set; }

    [JsonProperty("minimum_orbit_intersection")]
    public string? MinimumOrbitIntersection { get; set; }

    [JsonProperty("eccentricity")]
    public string? Eccentricity { get; set; }

    [JsonProperty("semi_major_axis")]
    public string? SemiMajorAxis { get; set; }

    [JsonProperty("inclination")]
    public string? Inclination { get; set; }

    [JsonProperty("orbital_period")]
    public string? OrbitalPeriod { get; set; }

    [JsonProperty("perihelion_distance")]
    public string? PerihelionDistance { get; set; }

    [JsonProperty("aphelion_distance")]
    public string? AphelionDistance { get; set; }

    [JsonProperty("mean_motion")]
    public string? MeanMotion { get; set; }

    [JsonProperty("orbit_class")]
    public OrbitClass? OrbitClass { get; set; }
}