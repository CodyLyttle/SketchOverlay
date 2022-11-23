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
}