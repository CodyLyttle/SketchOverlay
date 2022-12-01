using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingToolCollection<TDrawing, TImageSource, TColor> 
    : IReadOnlyCollection<DrawingToolInfo<TDrawing, TImageSource, TColor>>
{
    DrawingToolInfo<TDrawing, TImageSource, TColor> SelectedToolInfo { get; set; }

    TTool GetTool<TTool>() where TTool : IDrawingTool<TDrawing, TColor>;
}