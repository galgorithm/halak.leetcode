partial class Solution
{
    public int MinSteps(int n)
    {
        return n > 1 ? Invoke(n) : 0;

        int Invoke(int x)
        {
            for (var i = 2; i * i <= x; i++)
            {
                if (x % i == 0)
                    return Invoke(x / i) + i;
            }

            return x;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/2-keys-keyboard/")]
    [NUnit.Framework.TestCase(1, ExpectedResult = 0)]
    [NUnit.Framework.TestCase(2, ExpectedResult = 2)]
    [NUnit.Framework.TestCase(3, ExpectedResult = 3)]
    [NUnit.Framework.TestCase(5, ExpectedResult = 5)]
    [NUnit.Framework.TestCase(9, ExpectedResult = 6)]
    [NUnit.Framework.TestCase(15, ExpectedResult = 8)]
    public object MinSteps(params object[] args) => InvokeTest();
}
