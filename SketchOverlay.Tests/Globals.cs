using CommunityToolkit.Mvvm.Messaging;

namespace SketchOverlay.Tests;

internal static class Globals
{
    public static IMessenger Messenger = WeakReferenceMessenger.Default;
}
