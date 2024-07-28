using System.Windows.Input;
using ReactiveUI;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class FavoritesViewModel : ViewModelBase
{
    public FavoritesViewModel() { }

    public FavoritesViewModel(NasaApiService nasaApiService, FavoritesDbService favoritesDbService)
    {
        _apodFavoritesViewModel = new ApodFavoritesViewModel(favoritesDbService);
        _neoFavoritesViewModel = new NeoFavoritesViewModel(nasaApiService, favoritesDbService);

        _currentFavoritesViewModel = _apodFavoritesViewModel;

        NavigateApodFavoritesCommand = ReactiveCommand.Create(NavigateApodFavorites);
        NavigateNeoFavoritesCommand = ReactiveCommand.Create(NavigateNeoFavorites);
    }

    private readonly ApodFavoritesViewModel _apodFavoritesViewModel;
    private readonly NeoFavoritesViewModel _neoFavoritesViewModel;
    private ViewModelBase _currentFavoritesViewModel;

    public ICommand NavigateApodFavoritesCommand { get; }
    public ICommand NavigateNeoFavoritesCommand { get; }

    public ViewModelBase CurrentFavoritesViewModel
    {
        get => _currentFavoritesViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentFavoritesViewModel, value);
    }

    private void NavigateApodFavorites()
    {
        CurrentFavoritesViewModel = _apodFavoritesViewModel;
    }

    private void NavigateNeoFavorites()
    {
        CurrentFavoritesViewModel = _neoFavoritesViewModel;
    }

    public override string ToString()
    {
        return "Favorites";
    }
}