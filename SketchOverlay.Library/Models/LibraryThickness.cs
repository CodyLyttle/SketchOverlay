namespace SketchOverlay.Library.Models;

public struct LibraryThickness
{
    public LibraryThickness(double universalSize)
    {
        Left = universalSize;
        Top = universalSize;
        Right = universalSize;
        Bottom = universalSize;
    }

    public LibraryThickness(double left, double top)
    {
        Left = left;
        Top = top;
        Right = 0;
        Bottom = 0;
    }

    public LibraryThickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public double Left { get; set; }
    public double Top { get; set; }
    public double Right { get; set; }
    public double Bottom { get; set; }
}