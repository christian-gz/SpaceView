using System;
using SpaceView.Models;
using SpaceView.Models.NeoModel;

namespace SpaceView.Services;

public class NeoFavoriteEventArgs(Neo neo, bool added) : EventArgs
{
    public Neo Neo { get; } = neo;
    public bool Added { get; } = added;
}