using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.LibraryAdapters;

// Bypass XAML {x:Type} restriction on generic parameters.
internal class MauiDrawingToolInfo : DrawingToolInfo<IDrawable, ImageSource, Color>
{
    public MauiDrawingToolInfo(IDrawingTool<IDrawable, Color> tool, ImageSource iconSource, string name) 
        : base(tool, iconSource, name)
    {
    }
}