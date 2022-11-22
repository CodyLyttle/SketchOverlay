using SketchOverlay.Drawing.Tools;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Views;

public partial class DrawingToolWindow
{
    private readonly BrushTool _brushTool = new();
    private readonly LineTool _lineTool = new();
    private readonly RectangleTool _rectangleTool = new();

    public DrawingToolWindow()
    {
        InitializeComponent();
        BindingContext = MauiProgram.GetService<DrawingToolWindowViewModel>();

        HideWindow();
        clearButton.IsEnabled = false;
        redoButton.IsEnabled = false;
        undoButton.IsEnabled = false;

        undoButton.Clicked += (_,_) => RequestUndo?.Invoke(this, EventArgs.Empty);
        redoButton.Clicked += (_,_) => RequestRedo?.Invoke(this, EventArgs.Empty);
        clearButton.Clicked += (_,_) => RequestClear?.Invoke(this, EventArgs.Empty);

        brushToolButton.Clicked += (_, _) => PrimaryToolChanged?.Invoke(this, _brushTool);
        lineToolButton.Clicked += (_, _) => PrimaryToolChanged?.Invoke(this, _lineTool);
        rectangleToolButton.Clicked += (_, _) => PrimaryToolChanged?.Invoke(this, _rectangleTool);

        redButton.Clicked += (_, _) => PrimaryToolColorChanged?.Invoke(this, Colors.Red);
        greenButton.Clicked += (_, _) => PrimaryToolColorChanged?.Invoke(this, Colors.Green);
        blueButton.Clicked += (_, _) => PrimaryToolColorChanged?.Invoke(this, Colors.Blue);

        drawSizeSlider.ValueChanged += (_, args) => PrimaryToolDrawSizeChanged?.Invoke(this, args.NewValue);
    }

    public event EventHandler? RequestUndo;
    public event EventHandler? RequestRedo;
    public event EventHandler? RequestClear;
    public event EventHandler<IDrawingTool>? PrimaryToolChanged;
    public event EventHandler<Color>? PrimaryToolColorChanged;
    public event EventHandler<double>? PrimaryToolDrawSizeChanged;

    public bool IsDragging { get; private set; }

    public bool SetCanUndo(bool value) 
        => Dispatcher.Dispatch(()
            => undoButton.IsEnabled = value);

    public bool SetCanRedo(bool value)
        => Dispatcher.Dispatch(()
            => redoButton.IsEnabled = value);

    public bool SetCanClear(bool value) 
        => Dispatcher.Dispatch(() 
            => clearButton.IsEnabled = value);

    public void ShowWindow()
    {
        IsVisible = true;
    }

    public void BeginDragWindow(PointF position)
    {
        IsDragging = true;
        InputTransparent = true;
        MoveWindow(position);
    }

    public void ContinueDragWindow(PointF position)
    {
        if (!IsDragging)
            throw new InvalidOperationException(
                $"{nameof(ContinueDragWindow)} was called before {nameof(BeginDragWindow)}");

        MoveWindow(position);
    }

    public void EndDragWindow(PointF position)
    {
        if (!IsDragging)
            return;

        MoveWindow(position);
        InputTransparent = false;
        IsDragging = false;
    }

    public void HideWindow()
    {
        InputTransparent = false;
        IsVisible = false;
    }

    private void MoveWindow(PointF position)
    {
        // Width is -1 when called too early.
        double width = Width == -1
            ? WidthRequest
            : Width;

        double left = position.X - width / 2;
        double top = position.Y;
        Margin = new Thickness(left, top, 0, 0);
    }
}