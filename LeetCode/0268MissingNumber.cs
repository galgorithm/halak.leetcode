partial class Solution
{
    public int MissingNumber(int[] nums)
    {
        var x = ((nums.Length + 1) * nums.Length) / 2; // [1..N].Sum()
        for (var i = 0; i < nums.Length; i++)
            x -= nums[i];
        return x;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/missing-number/")]
    [NUnit.Framework.TestCase(new[] { 3, 0, 1 }, ExpectedResult = 2)]
    [NUnit.Framework.TestCase(new[] { 9, 6, 4, 2, 3, 5, 7, 0, 1 }, ExpectedResult = 8)]
    [NUnit.Framework.TestCase(new[] { 0, 1, 2 }, ExpectedResult = 3)]
    [NUnit.Framework.TestCase(new[] { 1, 2, 3 }, ExpectedResult = 0)]
    public object MissingNumber(params object[] args) => InvokeTest();
}
