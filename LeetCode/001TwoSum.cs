using System;
using System.Collections.Generic;

partial class Solution
{
    public int[] TwoSum(int[] nums, int target)
    {
        var indexes = new int[nums.Length];
        for (var i = 0; i < indexes.Length; i++)
            indexes[i] = i;
        Array.Sort(nums, indexes, int32Comparer);

        var minIndex = Array.BinarySearch(nums, target - nums[nums.Length - 1], int32Comparer);
        if (minIndex < 0)
            minIndex = ~minIndex;
        else
            return new[] { indexes[nums.Length - 1], indexes[minIndex] };

        var maxIndex = Array.BinarySearch(nums, target - nums[0], int32Comparer);
        if (maxIndex < 0)
            maxIndex = ~maxIndex;
        else
            return new[] { indexes[0], indexes[maxIndex] };

        for (var i = minIndex; i < maxIndex; i++)
        {
            var k = Array.BinarySearch(nums, i + 1, maxIndex - i - 1, target - nums[i], int32Comparer);
            if (k >= 0)
                return new[] { indexes[i], indexes[k] };
        }

        return null;
    }

    private static readonly IComparer<int> int32Comparer = Comparer<int>.Default;
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/two-sum/")]
    [NUnit.Framework.TestCase(new[] { 2, 7, 11, 15 }, 9, ExpectedResult = new[] { 0, 1 })]
    public int[] TwoSum(int[] nums, int target) => new Solution().TwoSum(nums, target);
}
