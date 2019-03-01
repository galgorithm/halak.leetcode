partial class Solution
{
    public int FirstMissingPositive(int[] nums)
    {
        for (var i = 0; i < nums.Length; i++)
        {
            var x = nums[i];
            if (0 < x && x <= nums.Length)
            {
                var swap = x - 1;
                if (i == swap)
                    continue;
                if (nums[swap] != x)
                {
                    var t = nums[swap];
                    nums[swap] = x;
                    nums[i] = t;
                    i--;
                }
                else
                    nums[i] = -1; // duplicated
            }
            else
                nums[i] = -1; // unnecessary
        }

        for (var i = 0; i < nums.Length; i++)
        {
            if (nums[i] == -1)
                return i + 1;
        }

        return nums.Length + 1;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/first-missing-positive/")]
    [NUnit.Framework.TestCase(new[] { 1, 2, 0 }, ExpectedResult = 3)]
    [NUnit.Framework.TestCase(new[] { 3, 4, -1, 1 }, ExpectedResult = 2)]
    [NUnit.Framework.TestCase(new[] { 7, 8, 9, 11, 12 }, ExpectedResult = 1)]
    [NUnit.Framework.TestCase(new[] { 5, 4, 3, 2, 1 }, ExpectedResult = 6)]
    [NUnit.Framework.TestCase(new[] { 1, 1 }, ExpectedResult = 2)]
    public object FirstMissingPositive(params object[] args) => InvokeTest();
}
