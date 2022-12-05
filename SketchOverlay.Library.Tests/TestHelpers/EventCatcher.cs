namespace SketchOverlay.Library.Tests.TestHelpers;

internal class EventCatcher
{
    public List<(object? s, object? e)> Received { get; } = new();

    public void OnReceived(object? sender, object? args)
    {
        Received.Add((sender, args));
    }
}
