using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.ViewModels.Tools;
public class PaintBrushToolViewModel<TDrawing, TColor>
{
    public const float DefaultStrokeSize = 4f;

    private readonly IPaintBrushTool<TDrawing, TColor> _tool;

    public PaintBrushToolViewModel(IColorPalette<TColor> colorPalette, IPaintBrushTool<TDrawing, TColor> tool)
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