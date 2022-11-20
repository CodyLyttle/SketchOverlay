using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.Input;

public class InputStateTracker
{
    private readonly LowLevelMouseHook _mouseHook;
    private readonly Dictionary<KeyValuePair<MouseButton, int?>, bool> _mouseButtonStates = new();
    private System.Drawing.Point _cursorPosition;
    private readonly object _buttonStateLock = new();
    private readonly object _cursorPositionLock = new();

    public InputStateTracker(LowLevelMouseHook mouseHook)
    {
        _mouseHook = mouseHook;
        _mouseHook.MouseMove += (_, args) => CursorPosition = args.Position;
        _mouseHook.MouseUp += (_, args) => SetButtonState(args, false);
        _mouseHook.MouseDown += (_, args) => SetButtonState(args, true);
    }

    public System.Drawing.Point CursorPosition
    {
        get
        {
            lock (_cursorPositionLock)
            {
                return _cursorPosition;
            }
        }
        private set
        {
            lock (_cursorPositionLock)
            {
                _cursorPosition = value;
            }
        }
    }

    public bool IsButtonDown(MouseButton button)
    {
        if (button is MouseButton.XButton)
            throw new ArgumentOutOfRangeException(nameof(button),
                $"Use method {nameof(IsXButtonDown)}");

        lock (_buttonStateLock)
        {
            KeyValuePair<MouseButton, int?> key = new(button, null);
            if (_mouseButtonStates.TryGetValue(key, out bool isPressed))
                return isPressed;

            _mouseButtonStates[key] = false;
            return false;
        }
    }

    public bool IsXButtonDown(int xButtonId)
    {
        lock (_buttonStateLock)
        {
            KeyValuePair<MouseButton, int?> key = new(MouseButton.XButton, xButtonId);
            if (_mouseButtonStates.TryGetValue(key, out bool isPressed))
                return isPressed;

            _mouseButtonStates[key] = false;
            return false;
        }
    }

    private void SetButtonState(MouseButtonEventArgs args, bool isPressed)
    {
        KeyValuePair<MouseButton, int?> key = new(args.Button, args.XButtonIndex);
        lock (_buttonStateLock)
        {
            _mouseButtonStates[key] = isPressed;
        }
    }
}