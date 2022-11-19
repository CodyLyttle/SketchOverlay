using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SketchOverlay.Native.Input.Mouse;

public class LowLevelMouseHook
{
    private const int LowLevelMouseHookId = 14;
    private readonly WindowsHook _hook;

    public LowLevelMouseHook()
    {
        _hook = new WindowsHook();
        _hook.SetWindowsHook(
            LowLevelMouseHookId,
            HookCallback,
            NativeHelpers.MainModuleHandle(),
            0);
    }

    public event EventHandler<MouseMoveArgs>? MouseMove;
    public event EventHandler<MouseButtonEventArgs>? MouseDown;
    public event EventHandler<MouseButtonEventArgs>? MouseUp;
    public event EventHandler<MouseWheelArgs>? MouseWheel;

    private IntPtr HookCallback(int nCode, UIntPtr wParam, IntPtr lParam)
    {
        var mouseMessage = (MouseMessage) wParam;
        var eventInfo = Marshal.PtrToStructure<LowLevelMouseEventInfo>(lParam);

        switch (mouseMessage)
        {
            case MouseMessage.MouseMove:
                InvokeMouseMove(eventInfo);
                break;
            case MouseMessage.LeftButtonDown:
                InvokeMouseDown(eventInfo, MouseButton.Left);
                break;
            case MouseMessage.LeftButtonUp:
                InvokeMouseUp(eventInfo, MouseButton.Left);
                break;
            case MouseMessage.RightButtonDown:
                InvokeMouseDown(eventInfo, MouseButton.Right);
                break;
            case MouseMessage.RightButtonUp:
                InvokeMouseUp(eventInfo, MouseButton.Right);
                break;
            case MouseMessage.MiddleButtonDown:
                InvokeMouseDown(eventInfo, MouseButton.Middle);
                break;
            case MouseMessage.MiddleButtonUp:
                InvokeMouseUp(eventInfo, MouseButton.Middle);
                break;
            case MouseMessage.XButtonDown:
                InvokeMouseDown(eventInfo, MouseButton.XButton);
                break;
            case MouseMessage.XButtonUp:
                InvokeMouseUp(eventInfo, MouseButton.XButton);
                break;
            case MouseMessage.MouseWheel:
                InvokeMouseWheel(eventInfo);
                break;
            default:
                Debug.WriteLine($"unhandled mouse message: {mouseMessage}");
                break;
        }

        if (nCode < 0)
            return _hook.CallNextHook(nCode, wParam, lParam);

        // TODO: Mark the message as handled, or pass along.
        return _hook.CallNextHook(nCode, wParam, lParam);
    }

    private void InvokeMouseMove(LowLevelMouseEventInfo info)
    {
        MouseMove?.Invoke(this, new MouseMoveArgs(info.Position));
    }

    private void InvokeMouseDown(LowLevelMouseEventInfo info, MouseButton button)
    {
        MouseDown?.Invoke(this, CreateMouseButtonEventArgs(info, button));
    }

    private void InvokeMouseUp(LowLevelMouseEventInfo info, MouseButton button)
    {
        MouseUp?.Invoke(this, CreateMouseButtonEventArgs(info, button));
    }

    private void InvokeMouseWheel(LowLevelMouseEventInfo info)
    {
        MouseWheelArgs args = new(info.Position, info.MouseData.High); // Scroll delta found in hi-order word.
        MouseWheel?.Invoke(this, args);
    }

    private static MouseButtonEventArgs CreateMouseButtonEventArgs(LowLevelMouseEventInfo info, MouseButton button)
    {
        return button == MouseButton.XButton
            ? new MouseButtonEventArgs(info.Position, info.MouseData.High) // xButtonIndex found in hi-order word. 
            : new MouseButtonEventArgs(info.Position, button);
    }
}