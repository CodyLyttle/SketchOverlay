namespace SketchOverlay.Library;

public class ValueConverterTypeException<TExpected> : ArgumentException
{
    public ValueConverterTypeException(object value) : base(
        $"Expected type \"{typeof(TExpected).Name}\", actual type \"{value.GetType().Name}\"")
    {
    }
}