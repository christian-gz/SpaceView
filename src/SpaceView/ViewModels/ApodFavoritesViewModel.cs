using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SpaceView.Models;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class ApodFavoritesViewModel : ViewModelBase
{
    public ApodFavoritesViewModel() { }
    public ApodFavoritesViewModel(FavoritesDbService favoritesDbService)
    {
        _favoritesDbService = favoritesDbService;
        _favoritesDbService.ApodFavoriteEvent += OnApodFavoriteEvent;

        ApodDetailViewModel = new ApodDetailViewModel();

        ApodDetailViewModel.ApodClosedEvent += (sender, args) => SelectedApod = null;
        ApodDetailViewModel.ApodRemovedEvent += OnApodRemovedEvent;


        var loadTask = _favoritesDbService.GetApods()
            .ContinueWith(task => new ObservableCollection<Apod>(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
        FavoriteApodsNew = new NotifyTaskCompletion<ObservableCollection<Apod>>(loadTask);
    }

    private readonly FavoritesDbService _favoritesDbService;
    private Apod? _selectedApod;
    private bool _apodIsOpen;

    public NotifyTaskCompletion<ObservableCollection<Apod>> FavoriteApodsNew { get; }
    public ApodDetailViewModel ApodDetailViewModel { get; }
    public Apod? SelectedApod
    {
        get => _selectedApod;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedApod, value);
            ApodDetailViewModel.DetailApod = SelectedApod;
            ApodIsOpen = value != null;
        }
    }
    public bool ApodIsOpen
    {
        get => _apodIsOpen;
        set => this.RaiseAndSetIfChanged(ref _apodIsOpen, value);
    }

    private void OnApodFavoriteEvent(object? sender, ApodFavoriteEventArgs e)
    {
        if (FavoriteApodsNew.Result == null)
        {
            return;
        }

        if (e.Added)
        {
            if (!FavoriteApodsNew.Result.Any(apod => apod.Date == e.Apod.Date))
            {
                FavoriteApodsNew.Result.Add(e.Apod);
            }
        }
        else
        {
            var apodToRemove = FavoriteApodsNew.Result.FirstOrDefault(apod => apod.Date == e.Apod.Date);
            if (apodToRemove != null)
            {
                FavoriteApodsNew.Result.Remove(apodToRemove);
            }
        }
    }

    private async void OnApodRemovedEvent(object? o, EventArgs? e)
    {
        if (SelectedApod == null || FavoriteApodsNew.Result == null)
        {
            return;
        }

        var apodToRemove = SelectedApod;
        SelectedApod = null;
        FavoriteApodsNew.Result.Remove(apodToRemove);

        try
        {
            await _favoritesDbService.RemoveApod(apodToRemove);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error removing APOD: " + ex.Message);
            FavoriteApodsNew.Result.Add(apodToRemove);
        }
    }

    public override string ToString()
    {
        return "APOD";
    }
}