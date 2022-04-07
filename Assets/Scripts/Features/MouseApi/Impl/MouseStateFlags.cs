using System;

namespace Api.Map
{
    [Flags]
    public enum MouseStateFlags
    {
        None = 0,
        Zoom = 0x1,
        Hold = 0x2,
        Drag = 0x4,
        DblTap = 0x8,
        Any = Zoom|Hold|Drag|DblTap
    }
}