using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SketchOverlay.Native;

internal static class NativeHelpers
{
    public static uint CurrentThreadId()
    {
        return Native.GetCurrentThreadId();
    }

    public static IntPtr MainModuleHandle()
    {
        using Process currentProcess = Process.GetCurrentProcess();
        using ProcessModule mainModule = currentProcess.MainModule!;
        return Native.GetModuleHandle(mainModule.ModuleName!);
    }

    public static uint GetWindowThreadId(IntPtr windowHandle)
    {
        return Native.GetWindowThreadProcessId(windowHandle);
    }

    public static UIntPtr GetWindowProcessId(IntPtr windowHandle)
    {
        Native.GetWindowThreadProcessId(windowHandle, out UIntPtr processId);
        return processId;
    }

    public static NativeRect WindowRect(IntPtr hWnd)
    {
        if (!Native.GetWindowRect(hWnd, out NativeRect rect))
            throw new ArgumentOutOfRangeException(nameof(hWnd), "Invalid window handle");

        return rect;
    }

    private static class Native
    {
        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out UIntPtr processId);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out NativeRect rect);
    }
}