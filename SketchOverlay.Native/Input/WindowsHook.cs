using System.Runtime.InteropServices;

namespace SketchOverlay.Native.Input;

internal sealed class WindowsHook : IDisposable
{
    // Store the callback delegate to prevent early collection resulting in a NullReferenceException.
    private HookProcedure? _callbackObject;
    private IntPtr? _hookHandle;
    public bool IsHookSet { get; private set; }

    public void SetWindowsHook(int hookId, HookProcedure callback, IntPtr hookDllHandle, uint hookThreadId)
    {
        if (IsHookSet)
            throw new InvalidOperationException("Hook is already set");

        _callbackObject = callback;
        _hookHandle = Native.SetWindowsHookEx(hookId, callback, hookDllHandle, hookThreadId);
    }

    public IntPtr CallNextHook(int nCode, UIntPtr wParam, IntPtr lParam)
    {
        return Native.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_hookHandle == null)
            return;
            
        Native.UnhookWindowsHookEx((IntPtr)_hookHandle);
    }

    private static class Native
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int hookId, HookProcedure callback, IntPtr hookDllHandle,
            uint hookThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, UIntPtr wParam, IntPtr lParam);
    }
}