using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Messages.Actions;

namespace SketchOverlay.Messages;

public class DrawingWindowDragEventMessage : ValueChangedMessage<(DragAction, PointF position)>
{
    public DrawingWindowDragEventMessage(DragAction value, PointF position) : base((value, position))
    {
    }
}