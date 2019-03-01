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
    [NUnit.Framework.TestCase("[[1,2,3],[4,5,6],[7,8,9]]", ExpectedResult = "[[1,4,7],[2,5,8],[3,6,9]]")]
    [NUnit.Framework.TestCase("[[1,2,3],[4,5,6]]", ExpectedResult = "[[1,4],[2,5],[3,6]]")]
    public object Transpose(params object[] args) => InvokeTest();
}
