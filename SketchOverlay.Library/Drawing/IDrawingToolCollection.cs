using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingToolCollection<TDrawing, TImageSource> : IReadOnlyCollection<DrawingToolInfo<TDrawing, TImageSource>>
{
    public DrawingToolInfo<TDrawing, TImageSource> SelectedToolInfo { get; set; }
}