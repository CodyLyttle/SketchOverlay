using System.Drawing;

namespace SketchOverlay.Native;

public static class NativeExtensions
{
    public static Point RelativeToWindow(this Point absolutePoint, IntPtr windowHandle)
    {
        NativeRect rect = NativeHelpers.WindowRect(windowHandle);

        return new Point(
            absolutePoint.X - rect.Left,
            absolutePoint.Y - rect.Top);
    }
}