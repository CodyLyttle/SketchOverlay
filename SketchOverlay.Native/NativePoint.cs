using System.Drawing;
using System.Runtime.InteropServices;

namespace SketchOverlay.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct NativePoint
{
    public NativePoint(int x , int y)
    {
        X = x;
        Y = y;
    }
    
    public int X { get; set; }
    public int Y { get; set; }

    public static implicit operator Point(NativePoint native)
    {
        return new Point(native.X, native.Y);
    }
}