using System.Collections.Generic;

partial class Solution
{
    public int[,] UpdateMatrix(int[,] matrix)
    {
        const int Nothing = -1;

        var M = matrix.GetLength(0);
        var N = matrix.GetLength(1);
        var fillingPoints = new List<int>(M * N / 4);
        var temporaryFillingPoints = new List<int>(M * N / 4);
        for (var row = 0; row < M; row++)
        {
            for (var column = 0; column < N; column++)
            {
                if (matrix[row, column] == 0)
                {
                    matrix[row, column] = 0;
                    fillingPoints.Add((row * N) + column);
                }
                else
                    matrix[row, column] = Nothing;
            }
        }

        var step = 1;
        while (fillingPoints.Count > 0)
        {
            temporaryFillingPoints.Clear();

            for (var i = 0; i < fillingPoints.Count; i++)
            {
                var index = fillingPoints[i];
                var row = index / N;
                var column = index % N;
                if (TryFill(row - 1, column, step))
                    temporaryFillingPoints.Add(((row - 1) * N) + (column));
                if (TryFill(row + 1, column, step))
                    temporaryFillingPoints.Add(((row + 1) * N) + (column));
                if (TryFill(row, column - 1, step))
                    temporaryFillingPoints.Add(((row) * N) + (column - 1));
                if (TryFill(row, column + 1, step))
                    temporaryFillingPoints.Add(((row) * N) + (column + 1));
            }

            SwapFillingPoints();
            step++;
        }

        return matrix;

        bool TryFill(int row, int column, int value)
        {
            if (0 <= row && row < M && 0 <= column && column < N && matrix[row, column] == Nothing)
            {
                matrix[row, column] = value;
                return true;
            }
            else
                return false;
        }

        void SwapFillingPoints()
        {
            var t = temporaryFillingPoints;
            temporaryFillingPoints = fillingPoints;
            fillingPoints = t;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/01-matrix/")]
    [NUnit.Framework.TestCaseSource(nameof(UpdateMatrixArgs))]
    public int[,] UpdateMatrix(int[,] matrix) => new Solution().UpdateMatrix(matrix);

    static System.Collections.IEnumerable UpdateMatrixArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[,]
            {
                 { 0, 0, 0 },
                 { 0, 1, 0 },
                 { 1, 1, 1 },
             }).Returns(new[,]
            {
                 { 0, 0, 0 },
                 { 0, 1, 0 },
                 { 1, 2, 1 },
             });

            yield return new NUnit.Framework.TestCaseData((object)new[,]
            {
                 { 0 },
                 { 0 },
                 { 0 },
                 { 0 },
                 { 0 },
                 { 1 },
                 { 1 },
                 { 1 },
                 { 1 },
             }).Returns(new[,]
            {
                 { 0 },
                 { 0 },
                 { 0 },
                 { 0 },
                 { 0 },
                 { 1 },
                 { 2 },
                 { 3 },
                 { 4 },
             });
        }
    }
}
