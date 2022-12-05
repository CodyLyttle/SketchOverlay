namespace SketchOverlay.Library.Tests.TestHelpers;

internal class EventCatcher<TArgs>
{
    public List<(object? s, TArgs e)> Received { get; } = new();

    public void OnReceived(object? sender, TArgs args)
    {
        Received.Add((sender, args));
    }
}
