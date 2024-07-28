using System.Windows.Input;
using ReactiveUI;
using SpaceView.Configuration;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(){ }
    public MainWindowViewModel(IConfig config)
    {
        NasaApiService nasaApiService = new NasaApiService(config);
        FavoritesDbService favoritesDbService = new FavoritesDbService(config);

        _apodViewModel = new ApodViewModel(nasaApiService, favoritesDbService);
        _neoFeedViewModel = new NeoFeedViewModel(nasaApiService, favoritesDbService);
        _favoritesViewModel = new FavoritesViewModel(nasaApiService, favoritesDbService);

        _currentViewModel = _apodViewModel;

        NavigateApodCommand = ReactiveCommand.Create(NavigateApod);
        NavigateNeoFeedCommand = ReactiveCommand.Create(NavigateNeoFeed);
        NavigateFavoritesCommand = ReactiveCommand.Create(NavigateFavorites);
    }

    private ViewModelBase _currentViewModel;
    private readonly ApodViewModel _apodViewModel;
    private readonly NeoFeedViewModel _neoFeedViewModel;
    private readonly FavoritesViewModel _favoritesViewModel;

    public ICommand NavigateApodCommand { get; set; }
    public ICommand NavigateNeoFeedCommand { get; set; }
    public ICommand NavigateFavoritesCommand { get; set; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    
    private void NavigateApod()
    {
        CurrentViewModel = _apodViewModel;
    }

    private void NavigateNeoFeed()
    {
        CurrentViewModel = _neoFeedViewModel;
    }

    private void NavigateFavorites()
    {
        CurrentViewModel = _favoritesViewModel;
    }
}