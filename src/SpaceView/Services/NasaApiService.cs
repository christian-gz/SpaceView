using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SpaceView.Models;
using Newtonsoft.Json;
using SpaceView.Configuration;
using SpaceView.Models.NeoModel;

namespace SpaceView.Services;

public class NasaApiService
{
    public NasaApiService(IConfig config)
    {
        _apiKey = config.ApiSettings.NasaApiKey;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.nasa.gov/")
        };
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public async Task<Apod> GetAstronomyPictureOfTheDayAsync(DateTime? dateTime, bool random = false)
    {
        try
        {
            var requestUri = $"planetary/apod?api_key={_apiKey}";

            requestUri += random ? $"&count=1" : "";
            requestUri += dateTime != null ? $"&date={dateTime.Value.ToString("yyyy-MM-dd")}" : "";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (random)
            {
                var apods = JsonConvert.DeserializeObject<List<Apod>>(jsonResponse);

                if (apods == null || apods.Count == 0)
                {
                    throw new Exception("Deserialization returned null.");
                }

                return apods.First();
            }
            else
            {
                var apod = JsonConvert.DeserializeObject<Apod>(jsonResponse);

                if (apod == null)
                {
                    throw new Exception("Deserialization returned null.");
                }

                return apod;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<NeoFeed> GetNearEarthObjectsAsync(DateTime startDate, int dayRange)
    {
        try
        {
            var requestUri = $"neo/rest/v1/feed?api_key={_apiKey}";

            requestUri += $"&start_date={startDate.ToString("yyyy-MM-dd")}";

            var endDate = startDate.AddDays(dayRange - 1);
            requestUri += $"&end_date={endDate.ToString("yyyy-MM-dd")}";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var neoFeed = JsonConvert.DeserializeObject<NeoFeed>(jsonResponse);

            if (neoFeed == null)
            {
                throw new Exception("Deserialization returned null.");
            }

            return neoFeed;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Neo> GetNearEarthObjectDetailAsync(int neoId)
    {
        try
        {
            var requestUri = $"neo/rest/v1/neo/{neoId}?api_key={_apiKey}";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var neo = JsonConvert.DeserializeObject<Neo>(jsonResponse);

            if (neo == null)
            {
                throw new Exception("Deserialization returned null.");
            }

            return neo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}