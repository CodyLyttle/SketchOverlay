using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Models;

namespace SketchOverlay.LibraryAdapters;

// Bypass XAML {x:Type} restriction on generic parameters.
internal class MauiDrawingToolInfo : DrawingToolInfo<IDrawable, ImageSource>
{
    public MauiDrawingToolInfo(IDrawingTool<IDrawable> tool, ImageSource iconSource, string name) : base(tool, iconSource, name)
    {
    }
}