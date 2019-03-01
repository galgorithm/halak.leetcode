partial class Solution
{
    public int[,] ImageSmoother(int[,] M)
    {
        var h = M.GetLength(0);
        var w = M.GetLength(1);
        if (w == 1 && h == 1)
            return M;

        var output = new int[h, w];
        if (w >= 3 && h >= 3)
        {
            output[0, 0] = Average4(0, 0, +1, +1);
            for (var x = 1; x < w - 1; x++)
                output[0, x] = AverageHorizontal6(x, 0, +1);
            output[0, w - 1] = Average4(w - 1, 0, -1, +1);

            for (var y = 1; y < h - 1; y++)
            {
                output[y, 0] = AverageVertical6(0, y, 1);
                for (var x = 1; x < w - 1; x++)
                    output[y, x] = Average9(x, y);
                output[y, w - 1] = AverageVertical6(w - 1, y, -1);
            }

            output[h - 1, 0] = Average4(0, h - 1, +1, -1);
            for (var x = 1; x < w - 1; x++)
                output[h - 1, x] = AverageHorizontal6(x, h - 1, -1);
            output[h - 1, w - 1] = Average4(w - 1, h - 1, -1, -1);
        }
        else
        {
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                    output[y, x] = AverageAwareEdge(x, y);
            }
        }

        return output;

        int Average9(int x, int y)
        {
            return (
                M[y - 1, x - 1] + M[y - 1, x + 0] + M[y - 1, x + 1] +
                M[y + 0, x - 1] + M[y + 0, x + 0] + M[y + 0, x + 1] +
                M[y + 1, x - 1] + M[y + 1, x + 0] + M[y + 1, x + 1]) / 9;
        }

        int AverageHorizontal6(int x, int y, int dy)
            => (M[y, x - 1] + M[y, x] + M[y, x + 1] + M[y + dy, x - 1] + M[y + dy, x] + M[y + dy, x + 1]) / 6;
        int AverageVertical6(int x, int y, int dx)
            => (M[y - 1, x] + M[y - 1, x + dx] + M[y, x] + M[y, x + dx] +M[y + 1, x] + M[y + 1, x + dx]) / 6;
        int Average4(int x, int y, int dx, int dy)
            => (M[y, x] + M[y, x + dx] + M[y + dy, x] + M[y + dy, x + dx]) / 4;

        int AverageAwareEdge(int x, int y)
        {
            var sum = 0;
            var count = 0;
            for (var yy = y - 1; yy <= y + 1; yy++)
            {
                if (yy < 0 || yy >= h)
                    continue;

                for (var xx = x - 1; xx <= x + 1; xx++)
                {
                    if (0 <= xx && xx < w)
                    {
                        sum += M[yy, xx];
                        count++;
                    }
                }
            }

            return sum / count;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/image-smoother/")]
    [NUnit.Framework.TestCase("[[1]]", ExpectedResult = "[[1]]")]
    [NUnit.Framework.TestCase("[[2,3]]", ExpectedResult = "[[2,2]]")]
    [NUnit.Framework.TestCase(@"[
        [1,1,1],
        [1,0,1],
        [1,1,1]
    ]", ExpectedResult = @"[
        [0,0,0],
        [0,0,0],
        [0,0,0]
    ]")]
    [NUnit.Framework.TestCase(@"[
        [2,3,4],
        [5,6,7],
        [8,9,10],
        [11,12,13],
        [14,15,16]
    ]", ExpectedResult = @"[
        [4,4,5],
        [5,6,6],
        [8,9,9],
        [11,12,12],
        [13,13,14]
    ]")]
    public object ImageSmoother(params object[] args) => InvokeTest();
}
