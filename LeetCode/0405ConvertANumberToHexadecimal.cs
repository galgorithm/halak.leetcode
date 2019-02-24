partial class Solution
{
    public string ToHex(int num)
    {
        if (num == 0)
            return "0";

        const int N = 8;

        var c = new char[N];
        var u = (uint)num;
        var index = 0;
        for (; index < N; index++)
        {
            if ((u & (0xF0000000 >> (index * 4))) != 0)
                break;
        }

        var count = 0;
        for (; index < N; index++, count++)
        {
            var n = (u & (0xF0000000 >> (index * 4))) >> ((N - index - 1) * 4);
            if (n < 10)
                c[count] = (char)('0' + (n));
            else
                c[count] = (char)('a' + (n - 10));
        }

        return new string(c, 0, count);
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/convert-a-number-to-hexadecimal/")]
    [NUnit.Framework.TestCase(26, ExpectedResult = "1a")]
    [NUnit.Framework.TestCase(-1, ExpectedResult = "ffffffff")]
    public object ToHex(int num) => InvokeTest();
}
