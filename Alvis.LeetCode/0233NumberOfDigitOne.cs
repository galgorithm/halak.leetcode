partial class Solution
{
    public int CountDigitOne(int n)
    {
        return 0;
    }
}

partial class Tests
{
    //[NUnit.Framework.Test(Description = "https://leetcode.com/problems/number-of-digit-one/")]
    //[NUnit.Framework.TestCase(1, ExpectedResult = 1)]
    //[NUnit.Framework.TestCase(9, ExpectedResult = 1)]
    //[NUnit.Framework.TestCase(10, ExpectedResult = 2)]
    //[NUnit.Framework.TestCase(99, ExpectedResult = 20)]
    //[NUnit.Framework.TestCase(13, ExpectedResult = 6)]
    //[NUnit.Framework.TestCase(17680233, ExpectedResult = 20322388)]
    //[NUnit.Framework.TestCase(53696656, ExpectedResult = 48249036)]
    //[NUnit.Framework.TestCase(53753129, ExpectedResult = 48281963)]
    //[NUnit.Framework.TestCase(40209688, ExpectedResult = 38203939)]
    //[NUnit.Framework.TestCase(27698629, ExpectedResult = 29649633)]
    //[NUnit.Framework.TestCase(75985535, ExpectedResult = 63594714)]
    //[NUnit.Framework.TestCase(51305173, ExpectedResult = 46157786)]
    //[NUnit.Framework.TestCase(24507281, ExpectedResult = 27753259)]
    //[NUnit.Framework.TestCase(24087525, ExpectedResult = 27445313)]
    //[NUnit.Framework.TestCase(3798672, ExpectedResult = 3299638)]
    //public object CountDigitOne(params object[] args) => InvokeTest();

    static void CountDigitOneTestCase(int count)
    {
        var random = new System.Random();
        for (var i = 0; i < count; i++)
        {
            var n = random.Next(100, 100000000);
            System.Console.WriteLine($"[NUnit.Framework.TestCase({n}, ExpectedResult = {CountDigitOne(n)})]");
        }

        int CountDigitOne(int n)
        {
            var sum = 0;
            System.Threading.Tasks.Parallel.For(1, n + 1,
                () => 0,
                (i, state, localSum) => localSum + Count(i),
                (localSum) => System.Threading.Interlocked.Add(ref sum, localSum));
            return sum;
        }

        int Count(int x)
        {
            var localSum = 0;
            for (; x > 0; x /= 10)
            {
                if (x % 10 == 1)
                    localSum++;
            }
            return localSum;
        }
    }
}
