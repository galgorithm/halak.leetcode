partial class Solution
{
    public double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        var total = nums1.Length + nums2.Length;

        var enumerator = Zip(nums1, nums2).GetEnumerator();
        var index = (total - 1) / 2;
        while (enumerator.MoveNext() && index-- > 0)
        {
        }

        if (total % 2 == 1)
        {
            return enumerator.Current;
        }
        else
        {
            var a = enumerator.Current;
            enumerator.MoveNext();
            var b = enumerator.Current;
            return (a + b) / 2.0;
        }

        System.Collections.Generic.IEnumerable<int> Zip(int[] a, int[] b)
        {
            var i = 0;
            var k = 0;
            while (i < a.Length && k < b.Length)
            {
                if (a[i] < b[k])
                {
                    yield return a[i++];
                }
                else
                {
                    yield return b[k++];
                }
            }

            while (i < a.Length)
            {
                yield return a[i++];
            }

            while (k < b.Length)
            {
                yield return b[k++];
            }
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/median-of-two-sorted-arrays/")]
    [NUnit.Framework.TestCase(new[] { 1, 3 }, new[] { 2 }, ExpectedResult = 2.0)]
    [NUnit.Framework.TestCase(new[] { 1, 2 }, new[] { 3, 4 }, ExpectedResult = 2.5)]
    public object FindMedianSortedArrays(params object[] args) => InvokeTest();
}
