using System.Collections.Generic;
using System.Linq;

partial class Solution
{
    public int[] GridIllumination(int N, int[][] lamps, int[][] queries)
    {
        var comparer = Comparer<long>.Default;
        var highDoubleWordComparer = Comparer<long>.Create((a, b) => (a >> 32).CompareTo(b >> 32));

        var points = lamps.Select(it => (x: it[0], y: it[1]));
        var sortedPoints1 = new List<long>(points.Select(p => Transform1(p.x, p.y)));
        var sortedPoints2 = new List<long>(points.Select(p => Transform2(p.x, p.y)));
        var sortedPoints3 = new List<long>(points.Select(p => Transform3(p.x, p.y)));
        var sortedPoints4 = new List<long>(points.Select(p => Transform4(p.x, p.y)));
        sortedPoints1.Sort(comparer);
        sortedPoints2.Sort(comparer);
        sortedPoints3.Sort(comparer);
        sortedPoints4.Sort(comparer);

        var illuminated = new int[queries.Length];
        for (var i = 0; i < queries.Length; i++)
        {
            var x = queries[i][0];
            var y = queries[i][1];

            illuminated[i] = Query(x, y) ? 1 : 0;

            Remove(x - 1, y - 1);
            Remove(x - 1, y + 0);
            Remove(x - 1, y + 1);
            Remove(x + 0, y - 1);
            Remove(x + 0, y + 0);
            Remove(x + 0, y + 1);
            Remove(x + 1, y - 1);
            Remove(x + 1, y + 0);
            Remove(x + 1, y + 1);
        }

        return illuminated;

        bool Query(int x, int y)
        {
            return
                sortedPoints1.BinarySearch(Transform1(x, y), highDoubleWordComparer) >= 0 ||
                sortedPoints2.BinarySearch(Transform2(x, y), highDoubleWordComparer) >= 0 ||
                sortedPoints3.BinarySearch(Transform3(x, y), highDoubleWordComparer) >= 0 ||
                sortedPoints4.BinarySearch(Transform4(x, y), highDoubleWordComparer) >= 0;
        }

        void Remove(int x, int y)
        {
            if (RemoveItem(sortedPoints1, Transform1(x, y)))
            {
                RemoveItem(sortedPoints2, Transform2(x, y));
                RemoveItem(sortedPoints3, Transform3(x, y));
                RemoveItem(sortedPoints4, Transform4(x, y));
            }
        }

        bool RemoveItem(List<long> list, long p)
        {
            var index = list.BinarySearch(p, comparer);
            if (index >= 0)
            {
                list.RemoveAt(index);
                return true;
            }
            else
                return false;
        }

        long Transform1(int x, int y) => ((long)x << 32) | (uint)y;
        long Transform2(int x, int y) => Transform1(y, x); // Rotate by 90deg
        long Transform3(int x, int y) => Transform1(x + y, x - y); // Rotate by 45deg (and scale 2/sqrt(2))
        long Transform4(int x, int y) => Transform1(x - y, x + y); // Rotate by 135deg (and scale 2/sqrt(2))
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/grid-illumination/")]
    [NUnit.Framework.TestCase(5, "[[0,0],[4,4]]", "[[1,1],[0,1]]", ExpectedResult = "[1,0]")]
    [NUnit.Framework.TestCase(5, "[[0,0],[4,4]]", "[[1,1],[1,0]]", ExpectedResult = "[1,0]")]
    [NUnit.Framework.TestCase(5, "[[3,9],[3,6],[8,3],[5,3],[8,1],[1,3],[5,9],[4,2]]", "[[1,9],[4,9],[7,1],[6,9]]", ExpectedResult = "[1,1,1,1]")]
    [NUnit.Framework.TestCase(1000000000, "1001GridIllumination001-Lamps.json", "1001GridIllumination001-Queries.json", ExpectedResult = "1001GridIllumination001-Answer.json")]
    public object GridIllumination(params object[] args) => InvokeTest();
}
