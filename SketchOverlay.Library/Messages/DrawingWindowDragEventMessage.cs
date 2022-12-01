using System.Drawing;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Messages;

public class DrawingWindowDragEventMessage : ValueChangedMessage<(DragAction, PointF position)>
{
    public DrawingWindowDragEventMessage(DragAction value, PointF position) : base((value, position))
    {
    }
}