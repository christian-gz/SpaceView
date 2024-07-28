using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using SpaceView.Configuration;
using Dapper;
using SpaceView.Models;
using SpaceView.Models.NeoModel;

namespace SpaceView.Services;

public class FavoritesDbService
{
    public FavoritesDbService(IConfig config)
    {
        _connectionString = config.ConnectionStrings.Default;
    }

    private readonly string _connectionString;
    private IDbConnection GetConnection() => new SQLiteConnection(_connectionString);

    public event EventHandler<NeoFavoriteEventArgs>? NeoFavoriteEvent;
    public event EventHandler<ApodFavoriteEventArgs>? ApodFavoriteEvent;

    public async Task<IEnumerable<Apod>> GetApods()
    {
        using (var connection = GetConnection())
        {
            var sql = "SELECT date, title, copyright, explanation, url, hd_url as HdUrl, media_type as MediaType FROM ApodFavorites";
            return await connection.QueryAsync<Apod>(sql);
        }
    }

    public async Task AddApod(Apod apod)
    {
        using (var connection = GetConnection())
        {
            var sql = "INSERT INTO ApodFavorites (date, title, copyright, explanation, url, hd_url, media_type) " +
                      "VALUES (@Date, @Title, @Copyright, @Explanation, @Url, @HdUrl, @MediaType);";
            await connection.ExecuteAsync(sql, apod);

            ApodFavoriteEvent?.Invoke(this, new ApodFavoriteEventArgs(apod, true));
        }
    }

    public async Task RemoveApod(Apod apod)
    {
        using (var connection = GetConnection())
        {
            var sql = "DELETE FROM ApodFavorites WHERE date = @date";
            await connection.ExecuteAsync(sql, new { date = apod.Date });

            ApodFavoriteEvent?.Invoke(this, new ApodFavoriteEventArgs(apod, false));
        }
    }

    public async Task<bool> IsFavoriteApod(Apod apod)
    {
        using (var connection = GetConnection())
        {
            var sql = "SELECT EXISTS (SELECT 1 FROM ApodFavorites WHERE date = @date)";
            return await connection.ExecuteScalarAsync<bool>(sql, new { date = apod.Date });
        }
    }

    public async Task<IEnumerable<Neo>> GetNeos()
    {
        using (var connection = GetConnection())
        {
            var sql = @"
                SELECT 
                    id, 
                    name, 
                    designation, 
                    estimated_diameter_meters_min AS 'EstimatedDiameter.Meters.Min', 
                    estimated_diameter_meters_max AS 'EstimatedDiameter.Meters.Max', 
                    potentially_hazardous AS IsPotentiallyHazardousAsteroid,
                    sentry_object AS IsSentryObject
                FROM NeoFavorites";
            return await connection.QueryAsync<Neo>(sql);
        }
    }

    public async Task AddNeo(Neo neo)
    {
        using (var connection = GetConnection())
        {
            var sql = @"
                INSERT INTO NeoFavorites 
                    (id, name, designation, estimated_diameter_meters_min, estimated_diameter_meters_max, potentially_hazardous, sentry_object)
                    VALUES (@ID, @Name, @Designation, @EstimatedDiameterMetersMax, @IsPotentiallyHazardousAsteroid, @IsPotentiallyHazardousAsteroid, @IsSentryObject);";

            var parameter = new
            {
                ID = neo.ID,
                Name = neo.Name,
                Designation = neo.Designation,
                EstimatedDiameterMetersMin = neo.EstimatedDiameter.Meters.Min,
                EstimatedDiameterMetersMax = neo.EstimatedDiameter.Meters.Max,
                IsPotentiallyHazardousAsteroid = neo.IsPotentiallyHazardousAsteroid,
                IsSentryObject = neo.IsSentryObject
            };

            await connection.ExecuteAsync(sql, parameter);

            NeoFavoriteEvent?.Invoke(this, new NeoFavoriteEventArgs(neo, true));
        }
    }

    public async Task RemoveNeo(Neo neo)
    {
        using (var connection = GetConnection())
        {
            var sql = "DELETE FROM NeoFavorites WHERE id = @id";
            await connection.ExecuteAsync(sql, new { id = neo.ID });

            NeoFavoriteEvent?.Invoke(this, new NeoFavoriteEventArgs(neo, false));
        }
    }

    public async Task<bool> IsFavoriteNeo(Neo neo)
    {
        using (var connection = GetConnection())
        {
            var sql = "SELECT EXISTS (SELECT 1 FROM NeoFavorites WHERE id = @id)";
            return await connection.ExecuteScalarAsync<bool>(sql, new { id = neo.ID });
        }
    }
}