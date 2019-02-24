using Interval = EraseOverlapIntervalsInterval;
using System.Linq;

partial class Solution
{
    public int EraseOverlapIntervals(Interval[] intervals)
    {
        var comparer = System.Collections.Generic.Comparer<int>.Default;

        System.Array.Sort(intervals, (a, b) => a.end - b.end);

        var sortedPoints = new System.Collections.Generic.List<int>(intervals.Length * 2);
        for (var i = 0; i < intervals.Length; i++)
        {
            var start = intervals[i].start;
            var end = intervals[i].end;
            var startIndex = sortedPoints.BinarySearch(start, comparer);
            var endIndex = sortedPoints.BinarySearch(end - 1, comparer);
            if (startIndex == endIndex && startIndex < 0)
            {
                var index = ~startIndex;
                if (index % 2 == 0)
                {
                    sortedPoints.Insert(index, start);
                    sortedPoints.Insert(index + 1, end - 1);
                }
            }
        }

        return intervals.Length - (sortedPoints.Count / 2);
    }
}

sealed class EraseOverlapIntervalsInterval
{
    public int start;
    public int end;
    public EraseOverlapIntervalsInterval() { start = 0; end = 0; }
    public EraseOverlapIntervalsInterval(int s, int e) { start = s; end = e; }

    public override string ToString() => $"[{start}, {end})";
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/non-overlapping-intervals/")]
    [NUnit.Framework.TestCaseSource(nameof(EraseOverlapIntervalsArgs))]
    public object EraseOverlapIntervals(Interval[] intervals) => InvokeTest();

    static System.Collections.IEnumerable EraseOverlapIntervalsArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[] { Range(1, 2), Range(2, 3), Range(3, 4), Range(1, 3) })
                .SetArgDisplayNames("A")
                .Returns(1);
            yield return new NUnit.Framework.TestCaseData((object)new[] { Range(1, 2), Range(1, 2), Range(1, 2) })
                .SetArgDisplayNames("B")
                .Returns(2);
            yield return new NUnit.Framework.TestCaseData((object)new[] { Range(1, 2), Range(2, 3) })
                .SetArgDisplayNames("C")
                .Returns(0);

            yield return new NUnit.Framework.TestCaseData((object)new[] { Range(1, 2), Range(10, 12), Range(5, 11), Range(11, 15) })
                .SetArgDisplayNames("D")
                .Returns(1);

            yield return new NUnit.Framework.TestCaseData((object)ReadInt32Array2D("0435EraseOverlapIntervals001.txt").Select(it => Range(it[0], it[1])).ToArray())
                .SetArgDisplayNames("E")
                .Returns(187);

            Interval Range(int start, int end) => new Interval(start, end);
        }
    }
}
