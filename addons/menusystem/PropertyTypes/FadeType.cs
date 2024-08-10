using System;

namespace MenySystem.addons.menusystem.PropertyTypes;
[Flags]
public enum FadeType
{
    In = 1 << 0,
    Out = 1 << 1
}