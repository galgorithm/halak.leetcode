partial class Solution
{
    public bool IsPowerOfThree(int n)
    {
        if (n < 0 || n % 2 == 0)
            return false;

        // Enumerable.Range(0, (int)Math.Floor(Math.Log(int.MaxValue, 3)) + 1).Select(i => Math.Pow(3, i))
        switch (n)
        {
            case 1:
            case 3:
            case 9:
            case 27:
            case 81:
            case 243:
            case 729:
            case 2187:
            case 6561:
            case 19683:
            case 59049:
            case 177147:
            case 531441:
            case 1594323:
            case 4782969:
            case 14348907:
            case 43046721:
            case 129140163:
            case 387420489:
            case 1162261467:
                return true;
            default:
                return false;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/power-of-three/")]
    [NUnit.Framework.TestCase(-2147483648, ExpectedResult = false)]
    [NUnit.Framework.TestCase(27, ExpectedResult = true)]
    [NUnit.Framework.TestCase(0, ExpectedResult = false)]
    [NUnit.Framework.TestCase(9, ExpectedResult = true)]
    [NUnit.Framework.TestCase(45, ExpectedResult = false)]
    public object IsPowerOfThree(params object[] args) => InvokeTest();
}
