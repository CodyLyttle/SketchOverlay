﻿using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfOverlayWindowViewModel : OverlayWindowViewModel<WpfDrawing, WpfDrawingOutput, WpfImageSource, WpfColor>
{
    public WpfOverlayWindowViewModel(ICanvasManager<WpfDrawingOutput> canvasManager, IMessenger messenger) 
        : base(canvasManager, messenger)
    {
    }
}