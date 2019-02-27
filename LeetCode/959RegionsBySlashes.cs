using System;
using System.Collections.Generic;

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

        var segments = new Dictionary<(int x, int y), int>(grid.Length * grid.Length);

        var size = grid.Length;
        for (var d = 0; d < size; d++)
        {
            AddSegment(d, 0, EAST);
            AddSegment(size, d, SOUTH);
            AddSegment(size - d, size, WEST);
            AddSegment(0, size - d, NORTH);
        }

        for (var y = 0; y < size; y++) 
        {
            for (var x = 0; x < size; x++)
            {
                switch (grid[y][x])
                {
                    case '/':
                        AddSegment(x + 1, y + 0, SOUTHWEST);
                        AddSegment(x + 0, y + 1, NORTHEAST);
                        break;
                    case '\\':
                        AddSegment(x + 0, y + 0, SOUTHEAST);
                        AddSegment(x + 1, y + 1, NORTHWEST);
                        break;
                }
            }
        }

        var regions = 0;
        while (segments.Count > 0)
        {
            var enumerator = segments.GetEnumerator();
            enumerator.MoveNext();
            var item = enumerator.Current;
            
            if (Traverse(item.Key, GetAnyDirection(item.Value)))
                regions++;
        }

        return regions;

        int GetAnyDirection(int directions)
        {
            for (var i = 0; i < 8; i++)
            {
                if ((directions & (1 << i)) != 0)
                    return i;
            }

            return 0;
        }

        void AddSegment(int x, int y, int direction)
        {
            if (segments.TryGetValue((x, y), out var directions) == false)
                directions = 0;
            directions |= (1 << direction);
            segments[(x, y)] = directions;
        }

        bool PopSegment(int x, int y, int direction)
        {
            if (segments.TryGetValue((x, y), out var directions) && 
                (directions & (1 << direction)) != 0)
            {
                directions &= ~(1 << direction);
                if (directions != 0)
                    segments[(x, y)] = directions;
                else
                    segments.Remove((x, y));
                return true;
            }
            else
                return false;
        }

        bool Traverse((int x, int y) startPosition, int direction)
        {
            var current = Step(startPosition, direction);
            for (;;)
            {
                var startDirection = (direction + 4 - 1) % 8; // opposite
                var stepped = false;
                for (var i = 0; i < 8; i++)
                {
                    direction = (8 + startDirection - i) % 8;
                    if (PopSegment(current.x, current.y, direction))
                    {
                        current = Step(current, direction);
                        stepped = true;
                        if (current  == startPosition)
                            return true;

                        break;
                    }
                }

                if (stepped == false)
                    return false;
            }
        }

        (int x, int y) Step((int x, int y) position, int direction)
        {
            switch (direction)
            {
                case EAST: return (position.x + 1, position.y);
                case SOUTHEAST: return (position.x + 1, position.y + 1);
                case SOUTH: return (position.x, position.y + 1);
                case SOUTHWEST: return (position.x - 1, position.y + 1);
                case WEST: return (position.x - 1, position.y);
                case NORTHWEST: return (position.x - 1, position.y - 1);
                case NORTH: return (position.x, position.y - 1);
                case NORTHEAST: return (position.x + 1, position.y - 1);
                default: return position;
            }
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/regions-cut-by-slashes/")]
    [NUnit.Framework.TestCaseSource(nameof(RegionsBySlashesArgs))]
    public object RegionsBySlashes(string[] grid) => InvokeTest();


    static System.Collections.IEnumerable RegionsBySlashesArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[]
            {
                @" /",
                @"/ ",
            }).Returns(2);

            yield return new NUnit.Framework.TestCaseData((object)new[]
            {
                @" /",
                @"  ",
            }).Returns(1);

            yield return new NUnit.Framework.TestCaseData((object)new[]
            {
                @"\/",
                @"/\",
            }).Returns(4);

            yield return new NUnit.Framework.TestCaseData((object)new[]
            {
                @"/\",
                @"\/",
            }).Returns(5);

            yield return new NUnit.Framework.TestCaseData((object)new[]
            {
                @"//",
                @"/ ",
            }).Returns(3);
        }
    }
}
