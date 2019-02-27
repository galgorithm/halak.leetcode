partial class Solution
{
    public char FindTheDifference(string s, string t)
    {
        System.Span<int> counter = stackalloc int['z' - 'a' + 1];
        for (var i = 0; i < s.Length; i++)
            counter[s[i] - 'a']++;
        for (var i = 0; i < t.Length; i++)
        {
            var result = --counter[t[i] - 'a'];
            if (result < 0)
                return t[i];
        }

        return '\0';
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/find-the-difference/")]
    [NUnit.Framework.TestCase("abcd", "abcde", ExpectedResult = 'e')]
    [NUnit.Framework.TestCase("zxcv", "azxcv", ExpectedResult = 'a')]
    public object FindTheDifference(string s, string t) => InvokeTest();
}
