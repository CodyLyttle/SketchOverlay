using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.Models;

public class MouseActionInfo
{
    public MouseActionInfo(MouseButton button, PointF cursorPos)
    {
        Button = button;
        CursorPosition = cursorPos;
    }
    
    public MouseButton Button { get; }
    public PointF CursorPosition { get; }
}