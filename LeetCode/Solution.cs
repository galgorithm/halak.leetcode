using System;

partial class Solution
{
    static void Draw(Action<IDebugPainter> handle) => DebugPainters.Draw(handle);
}
