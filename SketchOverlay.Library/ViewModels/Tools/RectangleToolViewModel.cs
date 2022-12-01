using System.Drawing;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.ViewModels.Tools;

public class RectangleToolViewModel<TDrawing>
{
    public const float DefaultStrokeSize = 4f;

    private readonly IRectangleTool<TDrawing> _tool;

    public RectangleToolViewModel(IColorPalette colorPalette, IRectangleTool<TDrawing> tool)
    {
        ColorPalette = colorPalette;
        _tool = tool;
        _tool.FillColor = colorPalette.SecondaryColor;
        _tool.StrokeColor = colorPalette.PrimaryColor;
        _tool.StrokeSize = DefaultStrokeSize;
    }

    public IColorPalette ColorPalette { get; set; }

    public Color FillColor
    {
        get => _tool.FillColor;
        set => _tool.FillColor = value;
    }

    public Color StrokeColor
    {
        get => _tool.StrokeColor;
        set => _tool.StrokeColor = value;
    }

    public float StrokeSize
    {
        get => _tool.StrokeSize;
        set => _tool.StrokeSize = value;
    }
}