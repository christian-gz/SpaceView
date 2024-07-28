using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using SpaceView.Models.NeoModel;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class NeoDetailViewModel : ViewModelBase
{
    public NeoDetailViewModel()
    {
        NeoDetail = new Neo()
        {
            Name = "Neo Name",
            ID = "12345",
            Designation = "12345 designation"
        };
        NeoDetail.CloseApproach = new List<CloseApproachData>()
        {
            new CloseApproachData()
            {
                CloseApproachDate = "2024-01-01",
                RelativeVelocity = new Velocity()
                {
                    KilometersPerSecond = 1.123456,
                    KilometersPerHour = 60.123456
                },
                MissDistance = new Distance()
                {
                    Astronomical = 50.1234567,
                    Lunar = 50.1234567,
                    Kilometers = 50000050.1234567
                }
            }
        };
    }
    public NeoDetailViewModel(NasaApiService nasaApiService, FavoritesDbService favoritesDbService)
    {
        _nasaApiService = nasaApiService;
        _favoritesDbService = favoritesDbService;

        _favoritesDbService.NeoFavoriteEvent += (o, e) =>
        {
            if (e.Neo.ID == NeoDetail?.ID)
            {
                IsFavorite = e.Added;
            }
        };

        GoBackCommand = ReactiveCommand.Create(CloseNeoDetail);
        AddNeoToFavoritesCommand = ReactiveCommand.CreateFromTask(AddNeoToFavorites);
        RemoveNeoFromFavoritesCommand = ReactiveCommand.CreateFromTask(RemoveNeoFromFavorites);
    }

    private readonly NasaApiService _nasaApiService;
    private readonly FavoritesDbService _favoritesDbService;
    private Neo? _neoDetail;
    private bool _requestIsExecuting;
    private bool _isFavorite;

    public ICommand GoBackCommand { get; }
    public ICommand AddNeoToFavoritesCommand { get; }
    public ICommand RemoveNeoFromFavoritesCommand { get; }
    public event EventHandler? NeoDetailClosed;
    public event EventHandler? NeoRemovedEvent;

    public Neo? NeoDetail
    {
        get => _neoDetail;
        set => this.RaiseAndSetIfChanged(ref _neoDetail, value);
    }
    public bool RequestIsExecuting
    {
        get => _requestIsExecuting;
        set => this.RaiseAndSetIfChanged(ref _requestIsExecuting, value);
    }
    public bool IsFavorite
    {
        get => _isFavorite;
        set => this.RaiseAndSetIfChanged(ref _isFavorite, value);
    }

    public async Task LoadNeoDetail(string id)
    {
        RequestIsExecuting = true;
        NeoDetail = null;
        
        try
        {
            NeoDetail = await _nasaApiService.GetNearEarthObjectDetailAsync(int.Parse(id));
            IsFavorite = await _favoritesDbService.IsFavoriteNeo(NeoDetail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        RequestIsExecuting = false;
    }

    private void CloseNeoDetail()
    {
        NeoDetailClosed?.Invoke(this, EventArgs.Empty);
    }

    private async Task AddNeoToFavorites()
    {
        try
        {
            if (NeoDetail == null)
            {
                return;
            }

            await _favoritesDbService.AddNeo(NeoDetail);
            IsFavorite = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task RemoveNeoFromFavorites()
    {
        try
        {
            if (NeoDetail == null)
            {
                return;
            }

            await _favoritesDbService.RemoveNeo(NeoDetail);
            IsFavorite = false;
            NeoRemovedEvent?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}