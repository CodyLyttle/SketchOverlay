using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.Input;

public interface IInputManger
{
    LowLevelMouseHook Events { get; }
    InputStateTracker State { get; }
}