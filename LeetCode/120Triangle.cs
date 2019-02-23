using System;
using System.Collections.Generic;

partial class Solution
{
    public int MinimumTotal(IList<IList<int>> triangle)
    {
        var cache = new int[triangle.Count][];
        for (var i = 0; i < cache.Length; i++)
        {
            cache[i] = new int[triangle[i].Count];
            for (int k = 0; k < cache[i].Length; k++)
                cache[i][k] = int.MaxValue;
        }

        return Scan(0, 0);

        int Scan(int row, int column)
        {
            if (row < triangle.Count && column < triangle[row].Count)
            {
                var cost = cache[row][column];
                if (cost != int.MaxValue)
                    return cost;

                cost = triangle[row][column];
                if (row < triangle.Count - 1)
                {
                    cost += Math.Min(
                        Scan(row + 1, column + 0),
                        Scan(row + 1, column + 1));
                }

                cache[row][column] = cost;
                return cost;
            }
            else
                return int.MaxValue;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/triangle/")]
    [NUnit.Framework.TestCaseSource(nameof(MinimumTotalArgs))]
    public object MinimumTotal(IList<IList<int>> triangle) => InvokeTest();

    static System.Collections.IEnumerable MinimumTotalArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new int[][]
            {
                new [] { 2 },
                new [] { 3, 4 },
                new [] { 6, 5, 7 },
                new [] { 4, 1, 8, 3 },
            }).SetArgDisplayNames("A").Returns(11);
        }
    }
}
