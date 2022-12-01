﻿using System.Drawing;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.ViewModels.Tools;
public class PaintBrushToolViewModel<TDrawing>
{
    public const float DefaultStrokeSize = 4f;

    private readonly IPaintBrushTool<TDrawing> _tool;

    public PaintBrushToolViewModel(IColorPalette colorPalette, IPaintBrushTool<TDrawing> tool)
    {
        ColorPalette = colorPalette;
        _tool = tool;
        _tool.StrokeColor = colorPalette.PrimaryColor;
        _tool.StrokeSize = DefaultStrokeSize;
    }

    public IColorPalette ColorPalette { get; set; }

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