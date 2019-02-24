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
    [NUnit.Framework.TestCaseSource(nameof(SingleNonDuplicateArgs))]
    public object SingleNonDuplicate(int[] nums) => InvokeTest();

    static System.Collections.IEnumerable SingleNonDuplicateArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[] { 1, 1, 2, 3, 3, 4, 4, 8, 8 }).Returns(2);
            yield return new NUnit.Framework.TestCaseData((object)new[] { 3, 3, 7, 7, 10, 11, 11 }).Returns(10);
            yield return new NUnit.Framework.TestCaseData((object)new[] { 1, 1, 2 }).Returns(2);
            yield return new NUnit.Framework.TestCaseData((object)new[] { 1, 2, 2 }).Returns(1);
        }
    }
}
