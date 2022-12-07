using System.Drawing;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Messages;

public class ToolsWindowDragEventMessage : ValueChangedMessage<(DragAction action, PointF position)>
{
    public ToolsWindowDragEventMessage(DragAction action, PointF position) : base((action, position))
    {
    }
}