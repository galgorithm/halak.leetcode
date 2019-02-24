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
    [NUnit.Framework.TestCaseSource(nameof(GridIlluminationArgs))]
    public object GridIllumination(int N, int[][] lamps, int[][] queries) => InvokeTest();

    static System.Collections.IEnumerable GridIlluminationArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData(5,
                new int[][] { new[] { 0, 0 }, new[] { 4, 4 } },
                new int[][] { new[] { 1, 1 }, new[] { 0, 1 } })
                .SetArgDisplayNames("Small1")
                .Returns(new[] { 1, 0 });

            yield return new NUnit.Framework.TestCaseData(5,
                new int[][] { new[] { 0, 0 }, new[] { 4, 4 } },
                new int[][] { new[] { 1, 1 }, new[] { 1, 0 } })
                .SetArgDisplayNames("Small2")
                .Returns(new[] { 1, 0 });

            yield return new NUnit.Framework.TestCaseData(10,
                new int[][] { P(3, 9), P(3, 6), P(8, 3), P(5, 3), P(8, 1), P(1, 3), P(5, 9), P(4, 2) },
                new int[][] { P(1, 9), P(4, 9), P(7, 1), P(6, 9) })
                .SetArgDisplayNames("Medium")
                .Returns(new[] { 1, 1, 1, 1 });

            yield return new NUnit.Framework.TestCaseData(1000000000,
                ReadInt32Array2D("1001GridIllumination001-Lamps.txt"),
                ReadInt32Array2D("1001GridIllumination001-Queries.txt"))
                .SetArgDisplayNames("Large1")
                .Returns(ReadInt32Array("1001GridIllumination001-Answer.txt"));

            int[] P(int x, int y) => new[] { x, y };
        }
    }
}
