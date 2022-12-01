using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.ViewModels.Tools;

public class LineToolViewModel<TDrawing, TColor>
{
    public const float DefaultStrokeSize = 4f;

    private readonly ILineTool<TDrawing, TColor> _tool;

    public LineToolViewModel(IColorPalette<TColor> colorPalette, ILineTool<TDrawing, TColor> tool)
    {
        ColorPalette = colorPalette;
        _tool = tool;
        _tool.StrokeColor = colorPalette.PrimaryColor;
        _tool.StrokeSize = DefaultStrokeSize;
    }

    public IColorPalette<TColor> ColorPalette { get; set; }

    public TColor StrokeColor
    {
        get => _tool.StrokeColor;
        set
        {
            // Prevent MAUI CollectionView from setting the StrokeColor to null on CollectionView initialization.
            if (value is null)
                return;

            ColorPalette.PrimaryColor = value;
            _tool.StrokeColor = value;
        }
    }

    public float StrokeSize
    {
        get => _tool.StrokeSize;
        set => _tool.StrokeSize = value;
    }
}