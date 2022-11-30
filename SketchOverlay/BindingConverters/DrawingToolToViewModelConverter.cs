using System.Globalization;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.BindingConverters;
internal class DrawingToolToViewModelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IDrawingTool<IDrawable> tool)
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{nameof(value)} must be of type {nameof(IDrawingTool<IDrawable>)}");

        return tool switch
        {
            MauiPaintBrushTool => MauiProgram.GetService<IPaintBrushTool<IDrawable>>(),
            MauiLineTool => MauiProgram.GetService<ILineTool<IDrawable>>(),
            MauiRectangleTool => MauiProgram.GetService<IRectangleTool<IDrawable>>(),
            _ => throw new NotImplementedException("Drawing tool conversion not implemented")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}