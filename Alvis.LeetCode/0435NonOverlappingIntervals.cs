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

[Newtonsoft.Json.JsonArray]
sealed class EraseOverlapIntervalsInterval
{
    public int start;
    public int end;
    public EraseOverlapIntervalsInterval() { start = 0; end = 0; }
    public EraseOverlapIntervalsInterval(int s, int e) { start = s; end = e; }
    [Newtonsoft.Json.JsonConstructor]
    public EraseOverlapIntervalsInterval(System.Collections.Generic.List<object> values) : this(System.Convert.ToInt32(values[0]), System.Convert.ToInt32(values[1])) { }

    public override string ToString() => $"[{start}, {end})";
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/non-overlapping-intervals/")]
    [NUnit.Framework.TestCase("[[1,2],[2,3],[3,4],[1,3]]", ExpectedResult = 1)]
    [NUnit.Framework.TestCase("[[1,2],[1,2],[1,2]]", ExpectedResult = 2)]
    [NUnit.Framework.TestCase("[[1,2],[2,3]]", ExpectedResult = 0)]
    [NUnit.Framework.TestCase("[[1,2],[10,12],[5,11],[11,15]]", ExpectedResult = 1)]
    [NUnit.Framework.TestCase("0435NonOverlappingIntervals001.json", ExpectedResult = 187)]
    public object EraseOverlapIntervals(params object[] args) => InvokeTest();
}
