namespace SketchOverlay.Native.Input;

internal struct TwoWordParam
{
    public TwoWordParam(int param)
    {
        (short low, short high) = Convert32(param);
        Low = low;
        High = high;
    }

    public TwoWordParam(IntPtr param)
    {
        (short low, short high) = IntPtr.Size == 8
            ? Convert64(param.ToInt64())
            : Convert32(param.ToInt32());

        Low = low;
        High = high;
    }

    public short Low { get; }
    public short High { get; }
    
    public override string ToString()
    {
        return $"LO:{Low} HI:{High}";
    }

    private static (short low, short high) Convert32(int param)
    {
        return ((short)(param & 0xffff), (short)((param >> 16) & 0xffff));
    }

    private static (short low, short high) Convert64(long param)
    {
        return ((short)(param & 0xffff), (short)((param >> 16) & 0xffff));
    }
}