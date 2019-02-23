partial class Solution
{
    public bool IsNumber(string s)
    {
        // trim leading whitespace
        var index = 0;
        for (; index < s.Length; index++)
        {
            if (s[index] != ' ')
                break;
        }

        // trim trailing whitespace
        var endIndex = s.Length - 1;
        for (; endIndex >= index; endIndex--)
        {
            if (s[endIndex] != ' ')
                break;
        }
        if (index > endIndex)
            return false;

        var count = endIndex + 1;
        if (s[index] == '+' || s[index] == '-')
            index++;
        if (s[index] == '.')
            return IsDigit(s, count, index + 1) && IsFractionalPart(s, count, index + 1);
        if (IsDigit(s, count, index) == false)
            return false;

        index = ScanDigits(s, count, index);
        if (index == count)
            return true;

        switch (s[index])
        {
            case '.': return (index + 1) == count || IsFractionalPart(s, count, index + 1);
            case 'e': return IsExpPart(s, count, index + 1);
            default: return false;
        }
    }

    static bool IsFractionalPart(string s, int count, int index)
    {
        index = ScanDigits(s, count, index);
        return index == count || (s[index] == 'e' && IsExpPart(s, count, index + 1));
    }

    static bool IsExpPart(string s, int count, int index)
    {
        if (index == count)
            return false;
        if (s[index] == '+' || s[index] == '-')
            index++;
        if (IsDigit(s, count, index) == false)
            return false;
        return ScanDigits(s, count, index) == count;
    }

    static int ScanDigits(string s, int count, int index)
    {
        for (; index < count; index++)
        {
            if (IsDigit(s[index]) == false)
                return index;
        }

        return count;
    }

    static bool IsDigit(string s, int count, int index) => index < count && IsDigit(s[index]);
    static bool IsDigit(char c) => '0' <= c && c <= '9';
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/valid-number/")]
    [NUnit.Framework.TestCase("0", ExpectedResult = true)]
    [NUnit.Framework.TestCase(" 0.1 " , ExpectedResult = true)]
    [NUnit.Framework.TestCase("abc" , ExpectedResult = false)]
    [NUnit.Framework.TestCase("1 a" , ExpectedResult = false)]
    [NUnit.Framework.TestCase("2e10" , ExpectedResult = true)]
    [NUnit.Framework.TestCase(" -90e3   " , ExpectedResult = true)]
    [NUnit.Framework.TestCase(" 1e" , ExpectedResult = false)]
    [NUnit.Framework.TestCase("e3" , ExpectedResult = false)]
    [NUnit.Framework.TestCase(" 6e-1" , ExpectedResult = true)]
    [NUnit.Framework.TestCase(" 99e2.5 " , ExpectedResult = false)]
    [NUnit.Framework.TestCase("53.5e93" , ExpectedResult = true)]
    [NUnit.Framework.TestCase(" --6 " , ExpectedResult = false)]
    [NUnit.Framework.TestCase("-+3" , ExpectedResult = false)]
    [NUnit.Framework.TestCase("95a54e53" , ExpectedResult = false)]
    [NUnit.Framework.TestCase(".1", ExpectedResult = true)]
    [NUnit.Framework.TestCase("3.", ExpectedResult = true)]
    [NUnit.Framework.TestCase("+.8", ExpectedResult = true)]
    public object IsNumber(string s) => InvokeTest();
}
