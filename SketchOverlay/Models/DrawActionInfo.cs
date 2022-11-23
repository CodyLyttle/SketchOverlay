using SketchOverlay.Messages.Actions;
using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.Models;

public class DrawActionInfo
{
    public DrawActionInfo(DrawAction action, MouseButton button, PointF cursorPos)
    {
        Action = action;
        Button = button;
        CursorPosition = cursorPos;
    }

    public DrawAction Action { get; }
    public MouseButton Button { get; }
    public PointF CursorPosition { get; }
}