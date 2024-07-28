using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using SpaceView.Models.NeoModel;
using SpaceView.Services;

namespace SpaceView.ViewModels;

public class NeoFavoritesViewModel : ViewModelBase
{
    public NeoFavoritesViewModel()
    {
        FavoriteNeosDesigner = new ObservableCollection<Neo>()
        {
            new Neo { ID = "110", Name = "Ast 900"},
            new Neo { ID = "119", Name = "Ast 909"},
            new Neo { ID = "120", Name = "Ast 910"},
            new Neo { ID = "129", Name = "Ast 919"},
        };
    }

    public NeoFavoritesViewModel(NasaApiService nasaApiService, FavoritesDbService favoritesDbService)
    {
        _favoritesDbService = favoritesDbService;
        _favoritesDbService.NeoFavoriteEvent += OnNeoFavoriteEvent;

        NeoDetailViewModel = new NeoDetailViewModel(nasaApiService, favoritesDbService);
        NeoDetailViewModel.NeoDetailClosed += OnNeoDetailClosed;
        NeoDetailViewModel.NeoRemovedEvent += OnNeoRemovedEvent;

        var loadTask = _favoritesDbService.GetNeos()
            .ContinueWith(task => new ObservableCollection<Neo>(task.Result),
                          TaskScheduler.FromCurrentSynchronizationContext());
        FavoriteNeosNew = new NotifyTaskCompletion<ObservableCollection<Neo>>(loadTask);

        OpenNeoDetailsCommand = ReactiveCommand.CreateFromTask<Neo?>(OpenNeoDetails);
    }

    private readonly FavoritesDbService _favoritesDbService;
    private Neo? _selectedNeo;
    private bool _neoDetailOpen;

    public ObservableCollection<Neo>? FavoriteNeosDesigner { get; set; }
    public NotifyTaskCompletion<ObservableCollection<Neo>> FavoriteNeosNew { get; }
    public NeoDetailViewModel NeoDetailViewModel { get; }
    public ICommand OpenNeoDetailsCommand { get; }

    public Neo? SelectedNeo
    {
        get => _selectedNeo;
        set => this.RaiseAndSetIfChanged(ref _selectedNeo, value);
    }

    public bool NeoDetailOpen
    {
        get => _neoDetailOpen;
        set => this.RaiseAndSetIfChanged(ref _neoDetailOpen, value);
    }

    private async Task OpenNeoDetails(Neo? neo)
    {
        if (neo?.ID == null)
        {
            return;
        }

        SelectedNeo = neo;
        NeoDetailOpen = true;
        await NeoDetailViewModel.LoadNeoDetail(SelectedNeo.ID);
    }

    /// <summary>
    /// React to NeoFavoriteEvent from FavoritesDbService by adding or removing the Neo
    /// to or from the favorite-list.
    /// </summary>
    private void OnNeoFavoriteEvent(object? sender, NeoFavoriteEventArgs e)
    {
        if (FavoriteNeosNew.Result == null)
        {
            return;
        }

        if (e.Added)
        {
            if (!FavoriteNeosNew.Result.Any(neo => neo.ID == e.Neo.ID))
            {
                FavoriteNeosNew.Result.Add(e.Neo);
            }
        }
        else
        {
            var neoToRemove = FavoriteNeosNew.Result.FirstOrDefault(neo => neo.ID == e.Neo.ID);
            if (neoToRemove != null)
            {
                FavoriteNeosNew.Result.Remove(neoToRemove);
            }
        }
    }

    /// <summary>
    /// React to NeoRemovedEvent from NeoDetailViewModel by closing the DetailView,
    /// NEO was already removed in the OnNeoFavoriteEvent-method.
    /// </summary>
    private void OnNeoRemovedEvent(object? o, EventArgs? e)
    {
        NeoDetailOpen = false;
    }

    private void OnNeoDetailClosed(object? o, EventArgs? e)
    {
        SelectedNeo = null;
        NeoDetailOpen = false;
    }

    public override string ToString()
    {
        return "NEO";
    }
}