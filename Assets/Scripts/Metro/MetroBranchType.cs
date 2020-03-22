using System;

namespace Metro
{
    [Flags]
    public enum MetroBranchType
    {
        Red = 1,
        Black = 2,
        Blue = 4,
        Green = 8,
    }
}