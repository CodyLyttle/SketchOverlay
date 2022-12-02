using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing.Tools;

public interface IDrawingToolCollection<TDrawing, TImageSource, TColor>
    : IReadOnlyCollection<DrawingToolInfo<TDrawing, TImageSource, TColor>>
{
    DrawingToolInfo<TDrawing, TImageSource, TColor> SelectedToolInfo { get; set; }

    TTool GetTool<TTool>() where TTool : IDrawingTool<TDrawing, TColor>;
}