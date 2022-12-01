using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.ViewModels.Tools;

public class RectangleToolViewModel<TDrawing, TColor>
{
    public const float DefaultStrokeSize = 4f;

    private readonly IRectangleTool<TDrawing, TColor> _tool;

    public RectangleToolViewModel(IColorPalette<TColor> colorPalette, IRectangleTool<TDrawing, TColor> tool)
    {
        ColorPalette = colorPalette;
        _tool = tool;
        _tool.FillColor = colorPalette.SecondaryColor;
        _tool.StrokeColor = colorPalette.PrimaryColor;
        _tool.StrokeSize = DefaultStrokeSize;
    }

    public IColorPalette<TColor> ColorPalette { get; set; }
    
    public TColor FillColor
    {
        get => _tool.FillColor;
        set
        {
            // Prevent MAUI CollectionView from setting the FillColor to null on CollectionView initialization.
            if (value is null)
                return;

            ColorPalette.SecondaryColor = value;
            _tool.FillColor = value;
        }
    }

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