using System;
using System.Windows.Input;
using ReactiveUI;
using SpaceView.Models;

namespace SpaceView.ViewModels;

public class ApodDetailViewModel : ViewModelBase
{
    public ApodDetailViewModel()
    {
        GoBackCommand = ReactiveCommand.Create(CloseApod);
        RemoveApodCommand = ReactiveCommand.Create(RemoveApod);
        SwitchOpenApodPictureCommand = ReactiveCommand.Create(() => IsApodPictureOpen = !IsApodPictureOpen);

        DetailApod = new Apod
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

    private Apod? _detailApod;
    private bool _isApodPictureOpen;

    public ICommand GoBackCommand { get; }
    public ICommand RemoveApodCommand { get; }
    public ICommand SwitchOpenApodPictureCommand { get; }
    public event EventHandler? ApodClosedEvent;
    public event EventHandler? ApodRemovedEvent;

    public Apod? DetailApod
    {
        get => _detailApod;
        set => this.RaiseAndSetIfChanged(ref _detailApod, value);
    }
    public bool IsApodPictureOpen
    {
        get => _isApodPictureOpen;
        set => this.RaiseAndSetIfChanged(ref _isApodPictureOpen, value);
    }

    private void CloseApod()
    {
        ApodClosedEvent?.Invoke(this, EventArgs.Empty);
    }

    private void RemoveApod()
    {
        ApodRemovedEvent?.Invoke(this, EventArgs.Empty);
    }
}