partial class Solution
{
    public int[][] Transpose(int[][] A)
    {
        var matrix = new int[A[0].Length][];
        
        for (var row = 0; row < matrix.Length; row++)
        {
            matrix[row] = new int[A.Length];
            for (var column = 0; column < A.Length; column++)
                matrix[row][column] = A[column][row];
        }

        return matrix;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/transpose-matrix/")]
    [NUnit.Framework.TestCaseSource(nameof(TransposeArgs))]
    public object Transpose(int[][] A) => InvokeTest();

    static System.Collections.IEnumerable TransposeArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new int[][] 
            {
                new[] {1, 2, 3},
                new[] {4, 5, 6},
                new[] {7, 8, 9},
            }).Returns(new int[][]
            {
                new[] {1, 4, 7},
                new[] {2, 5, 8},
                new[] {3, 6, 9},
            });

            yield return new NUnit.Framework.TestCaseData((object)new int[][] 
            {
                new[] {1, 2, 3},
                new[] {4, 5, 6},
            }).Returns(new int[][]
            {
                new[] {1, 4},
                new[] {2, 5},
                new[] {3, 6},
            });
        }
    }
}
