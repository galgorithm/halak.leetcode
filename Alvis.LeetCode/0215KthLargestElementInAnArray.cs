partial class Solution
{
    public int FindKthLargest(int[] nums, int k)
    {
        for (var i = 0; i < nums.Length; i++)
            CascadeUp(i + 1);
        for (var i = 1; i < k; i++)
            CascadeDown();

        return nums[0];

        void CascadeUp(int ordinal)
        {
            while (ordinal > 1)
            {
                var parentOrdinal = ordinal / 2;
                if (nums[parentOrdinal - 1] < nums[ordinal - 1])
                {
                    Swap(parentOrdinal, ordinal);
                    ordinal = parentOrdinal;
                }
                else
                    return;
            }
        }

        void CascadeDown()
        {
            var ordinal = 1;
            while ((ordinal * 2) - 1 < nums.Length)
            {
                var leftOrdinal = ordinal * 2;
                var rightOrdinal = leftOrdinal + 1;
                var leftIndex = leftOrdinal - 1;
                var rightIndex = rightOrdinal - 1;
                if (rightIndex >= nums.Length || nums[leftIndex] > nums[rightIndex])
                {
                    nums[ordinal - 1] = nums[leftIndex];
                    ordinal = leftOrdinal;
                }
                else
                {
                    nums[ordinal - 1] = nums[rightIndex];
                    ordinal = rightOrdinal;
                }

                nums[ordinal - 1] = int.MinValue;
            }
        }

        void Swap(int ordinal1, int ordinal2)
        {
            var t = nums[ordinal1 - 1];
            nums[ordinal1 - 1] = nums[ordinal2 - 1];
            nums[ordinal2 - 1] = t;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/kth-largest-element-in-an-array/")]
    [NUnit.Framework.TestCase("[2,1]", 2, ExpectedResult = 1)]
    [NUnit.Framework.TestCase("[3,2,1,5,6,4]", 2, ExpectedResult = 5)]
    [NUnit.Framework.TestCase("[3,2,3,1,2,4,5,5,6]", 4, ExpectedResult = 4)]
    [NUnit.Framework.TestCase("[3,3,3,3,3,3,3,3,3]", 8, ExpectedResult = 3)]
    public object FindKthLargest(params object[] args) => InvokeTest().IsUnordered();
}
