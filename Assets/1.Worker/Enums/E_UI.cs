using System;

[Flags]
public enum E_BlockOptions
{
    None = 0,
    ShowingColor = 1 << 0,
    BlockPanel = 1 << 1,
    UseBlockAction = 1 << 2,
}