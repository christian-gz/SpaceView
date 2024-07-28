using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpaceView.Models.NeoModel;

public class NeoFeed
{
    [JsonProperty("near_earth_objects")]
    public Dictionary<string, List<Neo>> NearEarthObjects { get; set; } = new();
}