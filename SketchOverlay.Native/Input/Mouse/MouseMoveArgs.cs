using System.Drawing;

namespace SketchOverlay.Native.Input.Mouse;

public class MouseMoveArgs : EventArgs
{
    internal MouseMoveArgs(Point pos)
    {
        Position = pos;
    }
    
    public Point Position { get; }
}