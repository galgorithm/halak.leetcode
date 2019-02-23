partial class Solution
{
    public int BulbSwitch(int n)
    {
        if (n <= 1)
            return n;

        var primes = new int[n + 1];
        primes[0] = 0;
        primes[1] = 1;
        var count = 1;
        for (var i = 2; i <= n; i++)
        {
            if (primes[i] != 0)
                count += CountFactors(i) % 2;
            else
            {
                for (var k = i; k <= n; k += i)
                    primes[k] = i;
            }
        }

        int CountFactors(int x)
        {
            var factors = 1;
            var p = primes[x];
            do
            {
                var c = 0;
                while (x >= p && x % p == 0)
                {
                    x /= p;
                    c++;
                }

                factors *= (c + 1);
                p = primes[x];
            } while (x > 1 && p > 1);
            return factors;
        }

        return count;
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
    public object BulbSwitch(int n) => InvokeTest();
}
