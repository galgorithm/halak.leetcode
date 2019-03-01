using System;
using System.Collections.Generic;

partial class Solution
{
    public IList<IList<int>> ThreeSum(int[] nums)
    {
        var comparer = Comparer<int>.Default;

        Array.Sort(nums, comparer);

        var set = new List<IList<int>>();
        for (var i = 0; i < nums.Length - 2 && nums[i] <= 0; i++)
        {
            if (i > 0 && nums[i - 1] == nums[i])
                continue;

            var a = nums[i];
            for (var k = i + 1; k < nums.Length - 1; k++)
            {
                if (k > i + 1 && nums[k - 1] == nums[k])
                    continue;

                var b = nums[k];
                if (a + b > 0 && b > 0)
                    break;

                var c = -(a + b);
                if (Array.BinarySearch(nums, k + 1, nums.Length - (k + 1), c, comparer) >= 0)
                    set.Add(new[] { a, b, c });
            }
        }

        return set;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/3sum/")]
    [NUnit.Framework.TestCaseSource(nameof(ThreeSumArgs))]
    public object ThreeSum(int[] nums) => InvokeTest();

    static System.Collections.IEnumerable ThreeSumArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[] { -1, 0, 1, 2, -1, -4 }).Returns(Unordered2D(new[]
            {
                new[] { -1, 0, 1 },
                new[] { -1, -1, 2 },
            }));

            yield return new NUnit.Framework.TestCaseData((object)new[] { -4, -2, -2, -2, 0, 1, 2, 2, 2, 3, 3, 4, 4, 6, 6 }).Returns(Unordered2D(new[]
            {
                new[] { -4, -2, 6 },
                new[] { -4, 0, 4 },
                new[] { -4, 1, 3 },
                new[] { -4, 2, 2 },
                new[] { -2, -2, 4 },
                new[] { -2, 0, 2 },
            }));
        }
    }
}
