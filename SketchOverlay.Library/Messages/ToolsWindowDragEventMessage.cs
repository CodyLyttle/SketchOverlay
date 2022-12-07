using System.Drawing;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Messages;

public class ToolsWindowDragEventMessage : ValueChangedMessage<(DragAction, PointF position)>
{
    public ToolsWindowDragEventMessage(DragAction value, PointF position) : base((value, position))
    {
    }
}