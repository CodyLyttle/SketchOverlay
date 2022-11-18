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
    
    private static class Native
    {
        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetModuleHandle(string moduleName);
    }
}