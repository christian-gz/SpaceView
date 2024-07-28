using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using SpaceView.Models;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class ApodViewModel : ViewModelBase
{
    public ApodViewModel()
    {
        TodayApod = new Apod
        {
            Title = "Gigantic Jets over Himalayan Mountains",
            Copyright = "Li Xuanhua",
            Date = "2024-01-01",
            Explanation = "Yes, but can your thunderstorm do this? Pictured here " +
                          "are gigantic jets shooting up from a thunderstorm " +
                          "last week toward the Himalayan Mountains in China and Bhutan.",
            MediaType = "image",
            Url = "https://apod.nasa.gov/apod/image/2406/AraDragons_Taylor_960.jpg"
        };
    }
    public ApodViewModel(NasaApiService nasaApiService, FavoritesDbService favoritesDbService)
    {
        _nasaApiService = nasaApiService;
        _favoritesDbService = favoritesDbService;

        _favoritesDbService.ApodFavoriteEvent += (o, e) =>
        {
            if (e.Apod.Date == TodayApod?.Date)
            {
                IsFavorite = e.Added;
            }
        };

        DateApod = new DateTimeOffset(DateTime.Today);

        IObservable<bool> canExecuteRequestsCommands =
            this.WhenAnyValue(vm => vm.RequestIsExecuting, isExecuting => !isExecuting);

        LoadApodCommand = ReactiveCommand.CreateFromTask(LoadApodAsync, canExecuteRequestsCommands);
        LoadRandomApodCommand = ReactiveCommand.CreateFromTask(LoadRandomApodAsync, canExecuteRequestsCommands);
        AddApodToFavoritesCommand = ReactiveCommand.CreateFromTask(AddApodToFavorites);
        RemoveApodFromFavoritesCommand = ReactiveCommand.CreateFromTask(RemoveApodFromFavorites);
        SwitchOpenApodPictureCommand = ReactiveCommand.Create(() => IsApodPictureOpen = !IsApodPictureOpen);

        Setup = new NotifyTaskCompletion<int>(LoadApodAsync());
    }

    private readonly NasaApiService _nasaApiService;
    private readonly FavoritesDbService _favoritesDbService;
    private Apod? _todayApod;
    private DateTimeOffset _dateApod;
    private bool _requestIsExecuting;
    private bool _isFavorite;
    private bool _hasError;
    private string? _errorMessage;
    private bool _isApodPictureOpen;

    public ICommand LoadApodCommand { get; }
    public ICommand LoadRandomApodCommand { get; }
    public ICommand AddApodToFavoritesCommand { get; }
    public ICommand RemoveApodFromFavoritesCommand { get; }
    public ICommand SwitchOpenApodPictureCommand { get; }
    public NotifyTaskCompletion<int> Setup { get; }

    public Apod? TodayApod
    {
        get => _todayApod;
        private set => this.RaiseAndSetIfChanged(ref _todayApod, value);
    }
    public DateTimeOffset DateApod
    {
        get => _dateApod;
        set => this.RaiseAndSetIfChanged(ref _dateApod, value);
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
    public bool IsApodPictureOpen
    {
        get => _isApodPictureOpen;
        set => this.RaiseAndSetIfChanged(ref _isApodPictureOpen, value);
    }

    private async Task<int> LoadApodAsync()
    {
        RequestIsExecuting = true;
        TodayApod = null;
        HasError = false;
        ErrorMessage = null;

        try
        {
            TodayApod = await _nasaApiService.GetAstronomyPictureOfTheDayAsync(DateApod.DateTime);
            IsFavorite = await _favoritesDbService.IsFavoriteApod(TodayApod);
        }
        catch (Exception e)
        {
            HasError = true;
            ErrorMessage = "Could not load the Astronomy Picture of the day: " + e.Message;
        }

        RequestIsExecuting = false;
        return 0;
    }

    private async Task LoadRandomApodAsync()
    {
        RequestIsExecuting = true;
        TodayApod = null;
        HasError = false;
        ErrorMessage = null;

        try
        {
            TodayApod = await _nasaApiService.GetAstronomyPictureOfTheDayAsync(null, true);
            IsFavorite = await _favoritesDbService.IsFavoriteApod(TodayApod);
        }
        catch (Exception e)
        {
            HasError = true;
            ErrorMessage = "Could not load the Astronomy Picture of the day: " + e.Message;
        }
        finally
        {
            RequestIsExecuting = false;
        }
    }

    private async Task AddApodToFavorites()
    {
        if (TodayApod == null)
        {
            return;
        }

        try
        {
            await _favoritesDbService.AddApod(TodayApod);
            IsFavorite = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task RemoveApodFromFavorites()
    {
        if (TodayApod == null)
        {
            return;
        }

        try
        {
            await _favoritesDbService.RemoveApod(TodayApod);
            IsFavorite = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public override string ToString()
    {
        return "APOD";
    }
}