namespace SketchOverlay.Library.Drawing.Tools;

public interface IPaintBrushTool<out TDrawing, TColor> : IDrawingTool<TDrawing>
{
     TColor StrokeColor { get; set; }
     float StrokeSize { get; set; }
}