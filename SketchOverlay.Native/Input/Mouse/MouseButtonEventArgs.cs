using System.Drawing;

namespace SketchOverlay.Native.Input.Mouse;

public class MouseButtonEventArgs : EventArgs
{
    public Point Position { get; init; }
    public MouseButton Button { get; init; }
}