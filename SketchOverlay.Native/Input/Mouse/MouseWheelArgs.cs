using System.Drawing;

namespace SketchOverlay.Native.Input.Mouse;

public class MouseWheelArgs : EventArgs
{
    internal MouseWheelArgs(Point pos, int delta)
    {
        Delta = delta;
        Position = pos;
        
        Direction = delta < 0
            ? ScrollDirection.Down
            : ScrollDirection.Up;
        
        Ticks = delta < 0
            ? delta / -120
            : delta / 120;
    }
    
    public ScrollDirection Direction { get; }
    public Point Position { get; }
    public int Delta { get; } 
    public int Ticks { get; }
}