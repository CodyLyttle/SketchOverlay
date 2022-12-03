using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Wpf.Drawing;

internal class DrawingStack : IDrawingStack<System.Windows.Media.Drawing, DrawingGroup>
{
    public int Count => Output.Children.Count;
    
    public DrawingGroup Output { get; } = new();

    public void Clear()
    {
        Output.Children.Clear();
    }

    public void PushDrawing(System.Windows.Media.Drawing drawing)
    {
        Output.Children.Add(drawing);
    }

    public System.Windows.Media.Drawing PopDrawing()
    {
        int lastIndex = Count - 1;
        System.Windows.Media.Drawing poppedDrawing = Output.Children[lastIndex];;
        Output.Children.RemoveAt(lastIndex);
        return poppedDrawing;
    }
}