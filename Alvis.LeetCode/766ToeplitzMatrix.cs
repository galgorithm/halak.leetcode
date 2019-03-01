partial class Solution
{
    public bool IsToeplitzMatrix(int[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var columns = matrix.GetLength(1);
        for (var row = 1; row < rows; row++)
        {
            for (var column = 1; column < columns; column++)
            {
                if (matrix[row - 1, column - 1] != matrix[row, column])
                    return false;
            }
        }

        return true;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/toeplitz-matrix/")]
    [NUnit.Framework.TestCase(@"[
        [1,2,3,4],
        [5,1,2,3],
        [9,5,1,2]
    ]", ExpectedResult = true)]
    [NUnit.Framework.TestCase(@"[
        [1,2],
        [2,2]
    ]", ExpectedResult = false)]
    [NUnit.Framework.TestCase(@"[
        [18],
        [66]
    ]", ExpectedResult = true)]
    public object IsToeplitzMatrix(params object[] args) => InvokeTest();
}
