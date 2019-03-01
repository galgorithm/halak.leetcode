partial class Solution
{
    public bool IsPowerOfFour(int n) => n > 0 && (n & (n - 1)) == 0 && (n & 0b01010101010101010101010101010101) != 0;
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/power-of-four/")]
    [NUnit.Framework.TestCase(-2147483648, ExpectedResult = false)]
    [NUnit.Framework.TestCase(0, ExpectedResult = false)]
    [NUnit.Framework.TestCase(16, ExpectedResult = true)]
    [NUnit.Framework.TestCase(256, ExpectedResult = true)]
    [NUnit.Framework.TestCase(5, ExpectedResult = false)]
    [NUnit.Framework.TestCase(512, ExpectedResult = false)]
    public object IsPowerOfFour(params object[] args) => InvokeTest();
}
