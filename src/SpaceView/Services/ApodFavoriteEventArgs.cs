using System;
using SpaceView.Models;

namespace SpaceView.Services;

public class ApodFavoriteEventArgs(Apod apod, bool added) : EventArgs
{
    public Apod Apod { get; } = apod;
    public bool Added { get; } = added;
}