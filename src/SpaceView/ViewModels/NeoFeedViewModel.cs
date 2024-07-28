using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using SpaceView.Models.NeoModel;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class NeoFeedViewModel : ViewModelBase
{
    public NeoFeedViewModel()
    {
        NeoFeed = new List<KeyValuePair<string, List<Neo>>>();
        NeoFeed.Add(new KeyValuePair<string, List<Neo>>("test", new List<Neo>
        {
            new Neo { ID = "110", Name = "Ast 900"},
            new Neo { ID = "119", Name = "Ast 909"}
        }));
        NeoFeed.Add(new KeyValuePair<string, List<Neo>>("test2", new List<Neo>
        {
            new Neo { ID = "120", Name = "Ast 910"},
            new Neo { ID = "129", Name = "Ast 919"}
        }));
    }

public NeoFeedViewModel(NasaApiService nasaApiService, FavoritesDbService favoritesDbService)
    {
        _nasaApiService = nasaApiService;
        NeoDetailViewModel = new NeoDetailViewModel(nasaApiService, favoritesDbService);

        DateNeoFeed = new DateTimeOffset(DateTime.Today);

        var canLoadNeoFeed =
            this.WhenAnyValue(vm => vm.DateRangeDays, days => days != 0);

        LoadNeoFeedCommand = ReactiveCommand.CreateFromTask(LoadNeoFeed, canLoadNeoFeed);
        OpenNeoDetailCommand = ReactiveCommand.CreateFromTask<string?>(OpenNeoDetail);

        NeoDetailViewModel.NeoDetailClosed += (sender, args) => NeoDetailOpen = false;
    }

    private readonly NasaApiService _nasaApiService;
    private List<KeyValuePair<string, List<Neo>>>? _neoFeed;
    private DateTimeOffset _dateNeoFeed;
    private bool _requestIsExecuting;
    private int _dateRangeDays;
    private bool _neoDetailOpen;
    private bool _hasError;
    private string? _errorMessage;

    public ICommand LoadNeoFeedCommand { get; }
    public ICommand OpenNeoDetailCommand { get; }
    public NeoDetailViewModel NeoDetailViewModel { get; }

    public List<KeyValuePair<string, List<Neo>>>? NeoFeed
    {
        get => _neoFeed;
        set => this.RaiseAndSetIfChanged(ref _neoFeed, value);
    }
    public DateTimeOffset DateNeoFeed
    {
        get => _dateNeoFeed;
        set => this.RaiseAndSetIfChanged(ref _dateNeoFeed, value);
    }
    public int DateRangeDays
    {
        get => _dateRangeDays;
        set => this.RaiseAndSetIfChanged(ref _dateRangeDays, value);
    }
    public int[] DateRangeOptions { get; } = [1, 3, 7];
    public bool RequestIsExecuting
    {
        get => _requestIsExecuting;
        set => this.RaiseAndSetIfChanged(ref _requestIsExecuting, value);
    }
    public bool NeoDetailOpen
    {
        get => _neoDetailOpen;
        set => this.RaiseAndSetIfChanged(ref _neoDetailOpen, value);
    }
    public bool HasError
    {
        get => _hasError;
        set => this.RaiseAndSetIfChanged(ref _hasError, value);
    }
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    private async Task LoadNeoFeed()
    {
        RequestIsExecuting = true;
        NeoFeed = null;
        HasError = false;
        ErrorMessage = null;

        try
        {
            var neoFeed = await _nasaApiService.GetNearEarthObjectsAsync(DateNeoFeed.DateTime, DateRangeDays);
            NeoFeed = neoFeed.NearEarthObjects.ToList();
        }
        catch (Exception e)
        {
            ErrorMessage = "Could not load the Near Earth Objects: " + e.Message;
            HasError = true;
        }

        RequestIsExecuting = false;
    }

    private async Task OpenNeoDetail(string? id)
    {
        if (id == null)
        {
            return;
        }

        NeoDetailOpen = true;
        await NeoDetailViewModel.LoadNeoDetail(id);
    }

    public override string ToString()
    {
        return "NEO";
    }
}