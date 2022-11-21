using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Drawing.Tools;

namespace SketchOverlay.Messages;
public class DrawingToolChangedMessage : ValueChangedMessage<IDrawingTool>
{
    public DrawingToolChangedMessage(IDrawingTool value) : base(value)
    {
    }
}