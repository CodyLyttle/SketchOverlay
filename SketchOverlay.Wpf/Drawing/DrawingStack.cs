using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Wpf.Drawing;

internal class DrawingStack : IDrawingStack<WpfDrawing, WpfDrawingOutput>
{
    public int Count => Output.Children.Count;
    
    public DrawingGroup Output { get; } = new();

    public void Clear()
    {
        Output.Children.Clear();
    }

    public void PushDrawing(WpfDrawing drawing)
    {
        Output.Children.Add(drawing);
    }

    public WpfDrawing PopDrawing()
    {
        int lastIndex = Count - 1;
        WpfDrawing poppedDrawing = Output.Children[lastIndex];;
        Output.Children.RemoveAt(lastIndex);
        return poppedDrawing;
    }
}