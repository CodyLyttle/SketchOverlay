using System.Drawing;

namespace SketchOverlay.Native.Input.Mouse;

public class MouseMoveArgs : EventArgs
{
    public Point Position { get; init; }
}