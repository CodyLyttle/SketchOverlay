using System.Globalization;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels.Tools;

namespace SketchOverlay.BindingConverters;
internal class DrawingToolToViewModelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IDrawingTool<IDrawable> tool)
            throw new ValueConverterTypeException<IDrawingTool<IDrawable>>(value);

        return tool switch
        {
            MauiPaintBrushTool => MauiProgram.GetService<PaintBrushToolViewModel<IDrawable>>(),
            MauiLineTool => MauiProgram.GetService<LineToolViewModel<IDrawable>>(),
            MauiRectangleTool => MauiProgram.GetService<RectangleToolViewModel<IDrawable>>(),
            _ => throw new NotImplementedException("Drawing tool conversion not implemented")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}