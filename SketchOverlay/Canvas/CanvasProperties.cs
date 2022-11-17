namespace SketchOverlay.Canvas;

public struct CanvasProperties
{
    public CanvasProperties()
    {
    }

    public Color FillColor { get; set; } = Colors.Blue;
    public Color StrokeColor { get; set; } = Colors.Red;
    public float StrokeSize { get; set; } = 2f;
}