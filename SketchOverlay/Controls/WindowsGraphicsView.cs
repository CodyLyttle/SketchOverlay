using System.Windows.Input;
using SketchOverlay.Library.Models;

#if WINDOWS
using Microsoft.Maui.Handlers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Point = Windows.Foundation.Point;
#endif

namespace SketchOverlay.Controls;

public class WindowsGraphicsView : GraphicsView
{
    public ICommand MouseDownCommand
    {
        get => (ICommand)GetValue(MouseDownCommandProperty);
        set => SetValue(MouseDownCommandProperty, value);
    }

    public ICommand MouseDragCommand
    {
        get => (ICommand)GetValue(MouseDragCommandProperty);
        set => SetValue(MouseDragCommandProperty, value);
    }

    public ICommand MouseUpCommand
    {
        get => (ICommand)GetValue(MouseUpCommandProperty);
        set => SetValue(MouseUpCommandProperty, value);
    }

#if WINDOWS
    private MouseButton? _pressedButton;
#endif

    public static void InitializeCustomHandlers()
    {
#if WINDOWS
        // "InputWinUI" isn't referencing anything, it's simply a key for the mapping.
        GraphicsViewHandler.Mapper.AppendToMapping("InputWinUI", (handler, view) =>
        {
            var instance = (WindowsGraphicsView)view;

            // There seems to be a limitation on pressing one button while another is already down,
            // that is, a button must be released before another pressed event will fire.
            // Use mouse hook if we need further functionality, such as right click to cancel current drawing action.
            handler.PlatformView.PointerPressed += (s, e) =>
            {
                if (!IsMouse(e)) return;
                instance.HandleMouseDown(e.GetCurrentPoint((UIElement)s));
                e.Handled = true;
            };

            handler.PlatformView.PointerMoved += (s, e) =>
            {
                if (!IsMouse(e)) return;
                instance.HandleMouseMove(e.GetCurrentPoint((UIElement)s));
                e.Handled = true;
            };

            handler.PlatformView.PointerReleased += (s, e) => CommonMouseUpHandler(instance, s, e);
            handler.PlatformView.PointerCaptureLost += (s, e) => CommonMouseUpHandler(instance, s, e);
            handler.PlatformView.PointerExited += (s, e) => CommonMouseUpHandler(instance, s, e);
        });

        void CommonMouseUpHandler(WindowsGraphicsView instance, object s, PointerRoutedEventArgs e)
        {
            if (!IsMouse(e)) return;
            instance.HandleMouseUp(e.GetCurrentPoint((UIElement)s));
            e.Handled = true;
        }
#endif
    }

#if WINDOWS
    private static bool IsMouse(PointerRoutedEventArgs e)
    {
        return e.Pointer.PointerDeviceType == PointerDeviceType.Mouse;
    }

    private void HandleMouseDown(PointerPoint pointer)
    {
        _pressedButton = GetPressedMouseButton(pointer);
        if (_pressedButton != null)
            MouseDownCommand.Execute(CreateMouseCommandParameter(_pressedButton, pointer.Position));
    }

    private void HandleMouseMove(PointerPoint pointer)
    {
        if (_pressedButton is null || 
            _pressedButton != GetPressedMouseButton(pointer)) 
            return;

        MouseDragCommand.Execute(CreateMouseCommandParameter(_pressedButton, pointer.Position));
    }

    private void HandleMouseUp(PointerPoint pointer)
    {
        if (_pressedButton is null) return;
        MouseUpCommand.Execute(CreateMouseCommandParameter(_pressedButton, pointer.Position));
        _pressedButton = null;
    }

    private static MouseButton? GetPressedMouseButton(PointerPoint pointInfo)
    {
        MouseButton? button = null;

        if (pointInfo.Properties.IsLeftButtonPressed)
            button = MouseButton.Left;
        else if (pointInfo.Properties.IsMiddleButtonPressed)
            button = MouseButton.Middle;
        else if (pointInfo.Properties.IsRightButtonPressed)
            button = MouseButton.Right;
        else if (pointInfo.Properties.IsXButton1Pressed)
            button = MouseButton.XButton1;
        else if (pointInfo.Properties.IsXButton2Pressed)
            button = MouseButton.XButton2;

        return button;
    }

    private static MouseActionInfo CreateMouseCommandParameter(MouseButton? button, Point cursorPos)
    {
        ArgumentNullException.ThrowIfNull(button);

        return new MouseActionInfo((MouseButton)button, 
            new System.Drawing.PointF(cursorPos._x, cursorPos._y));
    }
#endif

    public static readonly BindableProperty MouseDownCommandProperty =
        BindableProperty.CreateAttached(
            "MouseDownCommandProperty",
            typeof(ICommand),
            typeof(GraphicsView),
            null);

    public static readonly BindableProperty MouseDragCommandProperty =
        BindableProperty.CreateAttached(
            "MouseDragCommandProperty",
            typeof(ICommand),
            typeof(GraphicsView),
            null);

    public static readonly BindableProperty MouseUpCommandProperty =
        BindableProperty.CreateAttached(
            "MouseUpCommandProperty",
            typeof(ICommand),
            typeof(GraphicsView),
            null);
}