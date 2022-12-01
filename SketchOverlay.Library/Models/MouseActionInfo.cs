using System.Drawing;

namespace SketchOverlay.Library.Models;

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