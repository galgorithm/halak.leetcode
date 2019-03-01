using System;

partial class Solution
{
    public int CherryPickup(int[,] grid)
    {
        const int Block = -1;

        var N = grid.GetLength(0);
        var TotalSteps = (2 * N) - 1;

        var cache = new int[TotalSteps][,];
        for (var i = 0; i < cache.Length; i++)
        {
            var size = GetHeightMax(i) - GetHeightMin(i);
            cache[i] = new int[size, size];
        }

        var score = Scan(0, 0, 0);
        if (TryGetFromCache(cache.Length - 1, N - 1, N - 1, out var _))
            return score;
        else
            return 0;

        int Scan(int step, int h1, int h2)
        {
            var hMin = GetHeightMin(step);
            var hMax = GetHeightMax(step);
            if (step < cache.Length && (hMin <= h1 && h1 < hMax) && (hMin <= h2 && h2 < hMax))
            {
                if (TryGetFromCache(step, h1, h2, out var cachedValue))
                    return cachedValue;

                var y1 = h1;
                var x1 = step - y1;
                var y2 = h2;
                var x2 = step - y2;
                if (step < TotalSteps - 1)
                {
                    var cell1 = grid[y1, x1];
                    var cell2 = grid[y2, x2];
                    if (cell1 != Block && cell2 != Block)
                    {
                        var maxScore = Math.Max(
                            Math.Max(Scan(step + 1, h1 + 0, h2 + 0), Scan(step + 1, h1 + 1, h2 + 0)),
                            Math.Max(Scan(step + 1, h1 + 0, h2 + 1), Scan(step + 1, h1 + 1, h2 + 1)));
                        if (maxScore >= 0)
                            return StoreCache(step, h1, h2, maxScore + ((h1 == h2) ? cell1 : cell1 + cell2));
                    }

                    return StoreCache(step, h1, h2, -1);
                }
                else
                    return StoreCache(step, h1, h2, grid[y1, x1]);
            }
            else
                return -1;
        }

        bool TryGetFromCache(int step, int h1, int h2, out int value)
        {
            var hMin = GetHeightMin(step);
            var storedValue = cache[step][h1 - hMin, h2 - hMin];
            value = storedValue - 2;
            return storedValue != 0;
        }

        int StoreCache(int step, int h1, int h2, int value)
        {
            var hMin = GetHeightMin(step);
            cache[step][h1 - hMin, h2 - hMin] = value + 2;
            return value;
        }

        int GetHeightMin(int step) => Math.Max(step - (N - 1), 0);
        int GetHeightMax(int step) => Math.Min(step - (N - 1) + N, N);
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/cherry-pickup/")]
    [NUnit.Framework.TestCase(@"[
        [ 0, 1,-1],
        [ 1, 0,-1],
        [ 1, 1, 1]
    ]", ExpectedResult = 5)]
    [NUnit.Framework.TestCase(@"[
        [ 0, 1, 0, 1],
        [ 1, 0, 1, 0],
        [ 0, 1, 0, 0],
        [ 1, 0, 0, 0]
    ]", ExpectedResult = 4)]
    [NUnit.Framework.TestCase(@"[
        [ 1, 1,-1],
        [ 1,-1, 1],
        [-1, 1, 1]
    ]", ExpectedResult = 0)]
    [NUnit.Framework.TestCase(@"[
        [ 1, 1, 1, 1,-1,-1,-1, 1, 0, 0],
        [ 1, 0, 0, 0, 1, 0, 0, 0, 1, 0],
        [ 0, 0, 1, 1, 1, 1, 0, 1, 1, 1],
        [ 1, 1, 0, 1, 1, 1, 0,-1, 1, 1],
        [ 0, 0, 0, 0, 1,-1, 0, 0, 1,-1],
        [ 1, 0, 1, 1, 1, 0, 0,-1, 1, 0],
        [ 1, 1, 0, 1, 0, 0, 1, 0, 1,-1],
        [ 1,-1, 0, 1, 0, 0, 0, 1,-1, 1],
        [ 1, 0,-1, 0,-1, 0, 0, 1, 0, 0],
        [ 0, 0,-1, 0, 1, 0, 1, 0, 0, 1]
    ]", ExpectedResult = 22)]
    [NUnit.Framework.TestCase(@"[
        [ 1, 1, 1, 1, 0, 0, 0],
        [ 0, 0, 0, 1, 0, 0, 0],
        [ 0, 0, 0, 1, 0, 0, 1],
        [ 1, 0, 0, 1, 0, 0, 0],
        [ 0, 0, 0, 1, 0, 0, 0],
        [ 0, 0, 0, 1, 0, 0, 0],
        [ 0, 0, 0, 1, 1, 1, 1]
    ]", ExpectedResult = 15)]
    [NUnit.Framework.TestCase(@"[
        [ 1,-1, 1, 1, 1, 1, 1, 1,-1, 1],
        [ 1, 1, 1, 1,-1,-1, 1, 1, 1, 1],
        [-1, 1, 1,-1, 1, 1, 1, 1, 1, 1],
        [ 1, 1,-1, 1,-1, 1, 1, 1, 1, 1],
        [-1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
        [-1,-1, 1, 1, 1,-1, 1, 1,-1, 1],
        [ 1, 1, 1, 1, 1, 1, 1,-1, 1, 1],
        [ 1, 1, 1, 1,-1, 1,-1,-1, 1, 1],
        [ 1,-1, 1,-1,-1, 1, 1,-1, 1,-1],
        [-1, 1,-1, 1,-1, 1, 1, 1, 1, 1]
    ]", ExpectedResult = 23)]
    public object CherryPickup(params object[] args) => InvokeTest();
}
