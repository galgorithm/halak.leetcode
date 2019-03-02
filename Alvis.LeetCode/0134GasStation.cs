partial class Solution
{
    public int CanCompleteCircuit(int[] gas, int[] cost)
    {
        return 0;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/gas-station/")]
    [NUnit.Framework.TestCase("[1,2,3,4,5]", "[3,4,5,1,2]", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("[2,3,4]", "[3,4,3]", ExpectedResult = -1)]
    public object CanCompleteCircuit(params object[] args) => InvokeTest();
}
