using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Maui.ViewModels;

internal partial class MauiToolsWindowViewModel : ToolsWindowViewModel<MauiDrawing, MauiImageSource, MauiColor>
{
    public MauiToolsWindowViewModel(
        ICanvasProperties<MauiColor> canvasProps,
        IColorPalette<MauiColor> drawingColors,
        IDrawingToolCollection<MauiDrawing, MauiImageSource, MauiColor> drawingTools,
        ICanvasStateManager canvasStateManager,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, canvasStateManager, messenger)
    {
    }

    // BUG: Button.IsEnabled visual state stops updating after a few click events.
    // See: https://github.com/dotnet/maui/issues/7377
    // Workaround await Task.Yield() before sending message.
    [RelayCommand]
    private async void YieldClear()
    {
        await Task.Yield();
        ClearCommand.Execute(null);
    }

    [RelayCommand]
    private async void YieldRedo()
    {
        await Task.Yield();
        RedoCommand.Execute(null);
    }

    [RelayCommand]
    private async void YieldUndo()
    {
        await Task.Yield();
        UndoCommand.Execute(null);
    }
}