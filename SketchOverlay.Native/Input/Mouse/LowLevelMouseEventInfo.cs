﻿using System.Runtime.InteropServices;

namespace SketchOverlay.Native.Input.Mouse;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct LowLevelMouseEventInfo
{
    public LowLevelMouseEventInfo(NativePoint pos, int mouseData, int flags, int time, UIntPtr extraInfo)
    {
        Position = pos;
        MouseData = new TwoWordParam(mouseData);
        Flags = flags;
        Time = time;
        ExtraInfo = extraInfo;
    }
    
    public NativePoint Position { get;}
    public TwoWordParam MouseData { get; }
    public int Flags { get; }
    public int Time { get; }
    public UIntPtr ExtraInfo { get; }
}