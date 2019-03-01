using System;
using System.Collections.Generic;

partial class Solution
{
    public IList<IList<int>> CombinationSum(int[] candidates, int target)
    {
        Array.Sort(candidates);

        var result = new List<IList<int>>(candidates.Length);
        Traverse(new List<int>(), 0, 0);
        return result;

        void Traverse(List<int> context, int index, int initialSum)
        {
            var value = candidates[index];
            var factorMax = target / value;

            context.Capacity = Math.Max(context.Capacity, context.Count + factorMax + 1);

            var startIndex = context.Count;
            for (var i = 0; i <= factorMax; i++)
            {
                var sum = initialSum + (i * value);
                if (sum < target)
                {
                    if (index + 1 < candidates.Length)
                        Traverse(context, index + 1, sum);
                }
                else
                {
                    if (sum == target)
                        result.Add(context.ToArray());

                    break;
                }

                context.Add(value);
            }

            context.RemoveRange(startIndex, context.Count - startIndex);
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/combination-sum/")]
    [NUnit.Framework.TestCase("[2,3,6,7]", 7, ExpectedResult = "[[7],[2,2,3]]")]
    [NUnit.Framework.TestCase("[2,3,5]", 8, ExpectedResult = "[[2,2,2,2],[2,3,3],[3,5]]")]
    [NUnit.Framework.TestCase("[2,3,8,4]", 6, ExpectedResult = "[[2,2,2],[2,4],[3,3]]")]
    public object CombinationSum(params object[] args) => InvokeTest().IsUnordered();
}
