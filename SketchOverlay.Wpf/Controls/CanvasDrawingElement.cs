using System;
using System.Windows;
using System.Windows.Media;

namespace SketchOverlay.Wpf.Controls;

public class CanvasDrawingElement : FrameworkElement
{
    private readonly DrawingVisual _visualObject;

    public CanvasDrawingElement(WpfDrawing drawing)
    {
        _visualObject = new DrawingVisual();
        DrawingContext dc = _visualObject.RenderOpen();
        dc.DrawDrawing(drawing);
        dc.Close();
    }
        
    protected override int VisualChildrenCount => 1;

    protected override Visual GetVisualChild(int index)
    {
        if (index != 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        return _visualObject;
    }
}