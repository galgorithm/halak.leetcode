partial class Solution
{
    public int RegionsBySlashes(string[] grid)
    {
        const int EAST = 0;
        const int SOUTHEAST = 1;
        const int SOUTH = 2;
        const int SOUTHWEST = 3;
        const int WEST = 4;
        const int NORTHWEST = 5;
        const int NORTH = 6;
        const int NORTHEAST = 7;

        var N = grid.Length;
        var comparer = System.Collections.Generic.Comparer<(int x, int y, int orientation)>.Default;
        var segments = new System.Collections.Generic.List<(int x, int y, int orientation)>(N * N * 2);

        for (var d = 0; d < N; d++)
        {
            segments.Add((d, 0, EAST));
            segments.Add((N, d, SOUTH));
            segments.Add((N - d, N, WEST));
            segments.Add((0, N - d, NORTH));
        }

        for (var y = 0; y < N; y++)
        {
            for (var x = 0; x < N; x++)
            {
                switch (grid[y][x])
                {
                    case '/':
                        segments.Add((x + 1, y + 0, SOUTHWEST));
                        segments.Add((x + 0, y + 1, NORTHEAST));
                        break;
                    case '\\':
                        segments.Add((x + 0, y + 0, SOUTHEAST));
                        segments.Add((x + 1, y + 1, NORTHWEST));
                        break;
                }
            }
        }

        segments.Sort(comparer);

        var regions = 0;
        while (segments.Count > 0)
        {
            var segment = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);
            if (Traverse(segment))
                regions++;
        }

        return regions;

        bool Traverse((int x, int y, int orientation) startSegment)
        {
            var currentPoint = GetEndPoint(startSegment);
            var currentOrientation = startSegment.orientation;
            var crossProductSum = CrossProduct((startSegment.x, startSegment.y), currentPoint);
            while (segments.Count > 0)
            {
                var index = SweepCounterClockwise(currentPoint, (currentOrientation + 4 - 1) % 8);
                if (index != -1)
                {
                    var segment = segments[index];
                    segments.RemoveAt(index);

                    var nextPoint = GetEndPoint(segment);
                    crossProductSum += CrossProduct((currentPoint.x, currentPoint.y), (nextPoint.x, nextPoint.y));
                    if (nextPoint.x == startSegment.x && nextPoint.y == startSegment.y)
                        return crossProductSum > 0; // is clockwise
                    else
                    {
                        currentPoint = nextPoint;
                        currentOrientation = segment.orientation;
                    }
                }
                else
                    return false;
            }

            return false;
        }

        int SweepCounterClockwise((int x, int y) point, int startOrientation)
        {
            for (var i = startOrientation + 8; i > startOrientation; i--)
            {
                var orientation = i % 8;
                var index = segments.BinarySearch((point.x, point.y, orientation));
                if (index >= 0)
                    return index;
            }

            return -1;
        }

        (int x, int y) GetEndPoint((int x, int y, int orientation) segment)
        {
            switch (segment.orientation)
            {
                case EAST: return (segment.x + 1, segment.y);
                case SOUTHEAST: return (segment.x + 1, segment.y + 1);
                case SOUTH: return (segment.x, segment.y + 1);
                case SOUTHWEST: return (segment.x - 1, segment.y + 1);
                case WEST: return (segment.x - 1, segment.y);
                case NORTHWEST: return (segment.x - 1, segment.y - 1);
                case NORTH: return (segment.x, segment.y - 1);
                case NORTHEAST: return (segment.x + 1, segment.y - 1);
                default: return (segment.x, segment.y);
            }
        }

        int CrossProduct((int x, int y) p, (int x, int y) q) => (p.x * q.y) - (q.x * p.y);
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/regions-cut-by-slashes/")]
    [NUnit.Framework.TestCase(@"[
        "" /"",
        ""/ ""
    ]", ExpectedResult = 2)]
    [NUnit.Framework.TestCase(@"[
        "" /"",
        ""  ""
    ]", ExpectedResult = 1)]
    [NUnit.Framework.TestCase(@"[
        ""\\/"",
        ""/\\""
    ]", ExpectedResult = 4)]
    [NUnit.Framework.TestCase(@"[
        ""/\\"",
        ""\\/""
    ]", ExpectedResult = 5)]
    [NUnit.Framework.TestCase(@"[
        ""//"",
        ""/ ""
    ]", ExpectedResult = 3)]
    [NUnit.Framework.TestCase(@"[
        ""\\\\\\"",
        "" \\\\"",
        ""\\\\ ""
    ]", ExpectedResult = 4)]
    [NUnit.Framework.TestCase(@"[
        ""\\\\  /\\"",
        ""\\\\/\\\\\\"",
        ""/\\//\\/"",
        ""/\\ // "",
        ""//\\\\//"",
        ""\\//  \\""
    ]", ExpectedResult = 7)]
    [NUnit.Framework.TestCase(@"[
        ""\\//\\/\\//\\"",
        ""\\  /\\/ //"",
        ""//\\/ /\\\\ "",
        ""\\\\\\//\\///"",
        ""\\//// ///"",
        ""\\   / \\\\\\"",
        ""\\ /\\ /\\/\\"",
        ""/\\\\//  \\/"",
        "" ///\\/\\\\/""
    ]", ExpectedResult = 18)]
    public object RegionsBySlashes(params object[] args) => InvokeTest();
}
