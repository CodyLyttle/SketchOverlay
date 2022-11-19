using System.Drawing;
using System.Runtime.InteropServices;

namespace SketchOverlay.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct NativeRect
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public static implicit operator Rectangle(NativeRect native)
    {
        return new Rectangle(
            native.Left,
            native.Top,
            native.Right - native.Left,
            native.Bottom - native.Top);
    }
}