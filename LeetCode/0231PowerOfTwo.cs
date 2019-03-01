partial class Solution
{
    public bool IsPowerOfTwo(int n) => n > 0 && (n & (n - 1)) == 0;
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/power-of-two/")]
    [NUnit.Framework.TestCase(-2147483648, ExpectedResult = false)]
    [NUnit.Framework.TestCase(0, ExpectedResult = false)]
    [NUnit.Framework.TestCase(1, ExpectedResult = true)]
    [NUnit.Framework.TestCase(16, ExpectedResult = true)]
    [NUnit.Framework.TestCase(218, ExpectedResult = false)]
    public object IsPowerOfTwo(params object[] args) => InvokeTest();
}
