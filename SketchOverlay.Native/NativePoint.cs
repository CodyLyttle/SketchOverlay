using System.Drawing;
using System.Runtime.InteropServices;

namespace SketchOverlay.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct NativePoint
{
    public int X;
    public int Y;

    public static implicit operator Point(NativePoint native)
    {
        return new Point(native.X, native.Y);
    }
}