partial class Solution
{
    public int BulbSwitch(int x)
    {
        return Sqrt(x);

        int Sqrt(int n)
        {
            // from hacker's delight
            if (x <= 1)
                return x;

            var s = 2;
            var x1 = x - 1;
            if (x1 > 65535)
            {
                s *= 256;
                x1 /= 65536;
            }

            if (x1 > 255)
            {
                s *= 16;
                x1 /= 256;
            }

            if (x1 > 15)
            {
                s *= 4;
                x1 /= 16;
            }

            if (x1 > 3)
                s *= 2;

            var g0 = s;
            var g1 = (g0 + (x / s)) / 2;
            while (g1 < g0)
            {
                g0 = g1; // strictly decrease.
                g1 = (g0 + (x / g0)) / 2;
            }

            return g0;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/bulb-switcher/")]
    [NUnit.Framework.TestCase(3, ExpectedResult = 1)]
    [NUnit.Framework.TestCase(8, ExpectedResult = 2)]
    [NUnit.Framework.TestCase(12, ExpectedResult = 3)]
    [NUnit.Framework.TestCase(1000, ExpectedResult = 31)]
    [NUnit.Framework.TestCase(20631, ExpectedResult = 143)]
    [NUnit.Framework.TestCase(8618135, ExpectedResult = 2935)]
    [NUnit.Framework.TestCase(99999999, ExpectedResult = 9999)]
    public object BulbSwitch(params object[] args) => InvokeTest();
}
