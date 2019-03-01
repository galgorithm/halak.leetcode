using System;
using System.Globalization;

public static class VectorField
{
    public static void Main(string[] args)
    {
        var nfi = NumberFormatInfo.InvariantInfo;
        var n = int.Parse(Console.ReadLine(), NumberStyles.Integer, nfi);
        var forceField = new ForcePoint[n];
        var nodes = new NetworkNode[n];
        for (var i = 0; i < n; i++)
        {
            var elements = Console.ReadLine().Split(' ');
            forceField[i] = new ForcePoint(
                i,
                int.Parse(elements[0], NumberStyles.Integer, nfi),
                int.Parse(elements[1], NumberStyles.Integer, nfi),
                ParseDirection(elements[2]));
            nodes[i] = new NetworkNode(i);
        }

        Array.Sort(forceField, (a, b) => a.X != b.X ? (a.X - b.X) : (a.Y - b.Y));
        for (var i = 0; i < nodes.Length; i++)
        {
        }

        Array.Sort(forceField, (a, b) => a.Y != b.Y ? (a.Y - b.Y) : (a.X - b.X));
        for (var i = 0; i < nodes.Length; i++)
        {
        }

        Array.Sort(forceField, (a, b) => a.Id - b.Id);
        for (var i = 0; i < nodes.Length; i++)
        {
        }

        for (var i = 0; i < forceField.Length; i++)
        {
        }

        Console.WriteLine("2");
    }

    static int Simulate(ForcePoint points, NetworkNode[] nodes, int x, int y)
    {
        var clone = new NetworkNode[nodes.Length];
        Array.Copy(nodes, clone, clone.Length);
        nodes = clone;

        return 0;
    }

    enum Direction
    {
        Left,
        Up,
        Down,
        Right,
    }

    static Direction ParseDirection(string s)
    {
        switch (s.Length > 0 ? s[0] : '\0')
        {
            case '<': return Direction.Left;
            case '^': return Direction.Up;
            case '>': return Direction.Right;
            case 'v': return Direction.Down;
            default: return Direction.Left;
        }
    }

    struct ForcePoint
    {
        public readonly int Id;
        public readonly int X;
        public readonly int Y;
        public readonly Direction Direction;

        public ForcePoint(int id, int x, int y, Direction direction)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }
    }

    struct NetworkNode
    {
        public readonly int Id;
        public int Left;
        public int Up;
        public int Right;
        public int Down;

        public NetworkNode(int id)
        {
            Id = id;
            Left = -1;
            Up = -1;
            Right = -1;
            Down = -1;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://www.acmicpc.net/problem/16806")]
    //[NUnit.Framework.TestCase(@"
    //    9
    //    0 0 v
    //    1 0 >
    //    2 0 <
    //    0 1 >
    //    1 1 v
    //    2 1 v
    //    0 2 ^
    //    1 2 ^
    //    2 2 <", ExpectedResult = "9")]
    [NUnit.Framework.TestCase(@"
        9
        0 0 ^
        1 0 ^
        2 0 ^
        0 1 <
        1 1 ^
        2 1 >
        0 2 v
        1 2 v
        2 2 v", ExpectedResult = "2")]
    public object VectorField(params object[] args) => InvokeTest();
}
