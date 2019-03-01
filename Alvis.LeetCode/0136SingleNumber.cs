partial class Solution
{
    public int SingleNumber(int[] nums)
    {
        var x = 0;
        for (var i = 0; i < nums.Length; i++)
            x ^= nums[i];
        return x;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/single-number/")]
    [NUnit.Framework.TestCase(new[] { 2, 2, 1 }, ExpectedResult = 1)]
    [NUnit.Framework.TestCase(new[] { 4, 1, 2, 1, 2 }, ExpectedResult = 4)]
    public object SingleNumber(params object[] args) => InvokeTest();
}
