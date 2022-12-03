using SketchOverlay.Library.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MouseButton = SketchOverlay.Library.Models.MouseButton;

namespace SketchOverlay.Wpf.Controls
{
    public partial class DrawingCanvas : Canvas
    {
        public DrawingCanvas()
        {
            InitializeComponent();
        }

        public void AddDrawingElement(WpfDrawing drawing)
        {
            Children.Add(new CanvasDrawingElement(drawing));
        }

        public void Redraw()
        {
            InvalidateVisual();
        }

        #region Dependency Properties

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

        public static readonly DependencyProperty MouseUpCommandProperty =
            DependencyProperty.Register(
                "MouseUpCommand",
                typeof(ICommand),
                typeof(DrawingCanvas),
                null);

        public static readonly DependencyProperty MouseDragCommandProperty =
            DependencyProperty.Register(
                "MouseDragCommand",
                typeof(ICommand),
                typeof(DrawingCanvas),
                null);

        public static readonly DependencyProperty MouseDownCommandProperty =
            DependencyProperty.Register(
                "MouseDownCommand",
                typeof(ICommand),
                typeof(DrawingCanvas),
                null);

        #endregion

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            MouseDownCommand.Execute(
                CreateMouseCommandParameter(
                    e.ChangedButton.ToLibraryMouseButton(),
                    e.GetPosition(this)));
        }

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            InvokeCommandIfPressed(MouseButton.Left, e.LeftButton);
            InvokeCommandIfPressed(MouseButton.Right, e.RightButton);
            InvokeCommandIfPressed(MouseButton.Middle, e.MiddleButton);
            InvokeCommandIfPressed(MouseButton.XButton1, e.XButton1);
            InvokeCommandIfPressed(MouseButton.XButton2, e.XButton2);

            void InvokeCommandIfPressed(MouseButton button, MouseButtonState state)
            {
                if (state is MouseButtonState.Pressed)
                    MouseDragCommand.Execute(
                        CreateMouseCommandParameter(button, e.GetPosition(this)));
            }
        }

        private void UserControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            MouseUpCommand.Execute(
                CreateMouseCommandParameter(
                    e.ChangedButton.ToLibraryMouseButton(),
                    e.GetPosition(this)));
        }

        private static MouseActionInfo CreateMouseCommandParameter(MouseButton? button, Point cursorPos)
        {
            ArgumentNullException.ThrowIfNull(button);

            return new MouseActionInfo((MouseButton)button,
                new System.Drawing.PointF((float)cursorPos.X, (float)cursorPos.Y));
        }
    }
}
