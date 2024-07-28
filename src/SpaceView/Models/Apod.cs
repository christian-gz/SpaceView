using Newtonsoft.Json;

namespace SpaceView.Models;

public class Apod
{
    private string? _copyright;

    [JsonProperty("copyright")]
    public string? Copyright
    {
        get => _copyright;
        set => _copyright = value?.Trim();
    }

    [JsonProperty("date")]
    public string? Date { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("explanation")]
    public string? Explanation{ get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("hdurl")]
    public string? HdUrl { get; set; }

    [JsonProperty("media_type")]
    public string? MediaType { get; set; }
}