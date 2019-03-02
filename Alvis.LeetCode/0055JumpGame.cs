partial class Solution
{
    public bool CanJump(int[] nums)
    {
        var maxIndex = 0;
        for (var i = 0; i < nums.Length - 1 && maxIndex < nums.Length; i++)
        {
            if (nums[i] > 0 || maxIndex != i)
                maxIndex = System.Math.Max(maxIndex, i + nums[i]);
            else
                return false;
        }

        return maxIndex >= nums.Length - 1;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/jump-game/")]
    [NUnit.Framework.TestCase("[2,3,1,1,4]", ExpectedResult = true)]
    [NUnit.Framework.TestCase("[1,2,3]", ExpectedResult = true)]
    [NUnit.Framework.TestCase("[3,2,1,0,4]", ExpectedResult = false)]
    [NUnit.Framework.TestCase("[0]", ExpectedResult = true)]
    [NUnit.Framework.TestCase("[0,1]", ExpectedResult = false)]
    [NUnit.Framework.TestCase("0055JumpGame001.json", ExpectedResult = false)]
    public object CanJump(params object[] args) => InvokeTest();
}
