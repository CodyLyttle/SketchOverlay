namespace SketchOverlay.Library.Models;

public struct ControlMargin
{
    public ControlMargin()
    {
        Left = 0;
        Top = 0;
    }

    public ControlMargin(double left, double top)
    {
        Left = left;
        Top = top;
    }

    public double Left { get; set; }
    public double Top { get; set; }
}