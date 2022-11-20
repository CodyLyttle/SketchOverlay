using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.Input;

public class InputManager : IInputManger
{
    public InputManager()
    {
        Events = new LowLevelMouseHook();
        State = new InputStateTracker(Events);
    }

    public LowLevelMouseHook Events { get; }
    public InputStateTracker State { get; }
}