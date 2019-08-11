using Point = MaxPointsPoint;

partial class Solution
{
    public int MaxPoints(Point[] points)
    {
        // TOOD: SOLVE

        var gradients = new (int numerator, int denominator)[points.Length][];
        for (var i = 0; i < points.Length; i++)
            gradients[i] = new (int numerator, int denominator)[points.Length];

        for (var i = 0; i < points.Length - 1; i++)
        {
            var px = points[i].x;
            var py = points[i].y;
            for (var k = i + 1; k < points.Length; k++)
            {
                var dx = px - points[k].x;
                var dy = py - points[k].y;

                var gcd = GCD(Abs(dx), Abs(dy));
                if (gcd > 0)
                {
                    dx /= gcd;
                    dy /= gcd;
                }

                if ((dx < 0 && dy < 0) || (dx < 0 && dy > 0))
                {
                    dx = -dx;
                    dy = -dy;
                }

                gradients[i][k] = (dx, dy);
                gradients[k][i] = (dx, dy);
            }
        }

        var comparer = System.Collections.Generic.Comparer<(int, int)>.Default;
        var max = 0;
        for (var i = 0; i < gradients.Length; i++)
        {
            System.Array.Sort(gradients[i], comparer);
            var duplicated = 1;
            for (var k = 2; k < gradients.Length; k++)
            {
                if (gradients[i][k - 1].numerator == gradients[i][k].numerator &&
                    gradients[i][k - 1].denominator == gradients[i][k].denominator)
                    duplicated++;
                else
                {
                    max = System.Math.Max(max, duplicated);
                    duplicated = 1;
                }
            }

            max = System.Math.Max(max, duplicated + 1);
        }

        return max;

        int Abs(int x) => x > 0 ? x : -x;
        int GCD(int a, int b) => b != 0 ? GCD(b, a % b) : a;
    }
}

sealed class MaxPointsPoint
{
    public int x;
    public int y;
    public MaxPointsPoint() { x = 0; y = 0; }
    public MaxPointsPoint(int a, int b) { x = a; y = b; }

    public override string ToString() => $"({x}, {y})";
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/max-points-on-a-line/")]
    [NUnit.Framework.TestCase("[[1,1],[2,2],[3,3]]", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("[[1,1],[3,2],[5,3],[4,1],[2,3],[1,4]]", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("[[0,0],[0,0]]", ExpectedResult = 2)]
    [NUnit.Framework.TestCase("[[0,0],[94911151,94911150],[94911152,94911151]]", ExpectedResult = 2)]
    [NUnit.Framework.TestCase("[[1,1],[3,2],[5,3],[4,1],[2,3],[1,4]]", ExpectedResult = 4)]
    public object MaxPoints(params object[] args) => InvokeTest();
}
