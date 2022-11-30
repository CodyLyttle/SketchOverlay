using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingToolCollection<TDrawing, TImageSource> : IReadOnlyCollection<DrawingToolInfo<TDrawing, TImageSource>>
{
    DrawingToolInfo<TDrawing, TImageSource> SelectedToolInfo { get; set; }

    TTool GetTool<TTool>() where TTool : IDrawingTool<TDrawing>;
}