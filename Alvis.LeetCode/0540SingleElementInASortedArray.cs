partial class Solution
{
    public int SingleNonDuplicate(int[] nums)
    {
        var lo = 0;
        var hi = nums.Length;
        while (lo <= hi)
        {
            var i = lo + (hi - lo) / 2;
            if (i == 0 || i == nums.Length - 1)
                return nums[i];
            if (nums[i] == nums[i - 1])
                i--;
            else if (nums[i] != nums[i + 1])
                return nums[i];

            if (i % 2 != 0)
                hi = i - 1;
            else
                lo = i + 2;
        }

        return -1;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/single-element-in-a-sorted-array/")]
    [NUnit.Framework.TestCase("[1,1,2,3,3,4,4,8,8]", ExpectedResult = 2)]
    [NUnit.Framework.TestCase("[3,3,7,7,10,11,11]", ExpectedResult = 10)]
    [NUnit.Framework.TestCase("[1,1,2]", ExpectedResult = 2)]
    [NUnit.Framework.TestCase("[1,2,2]", ExpectedResult = 1)]
    public object SingleNonDuplicate(params object[] args) => InvokeTest();
}
