partial class Solution
{
    public int LengthOfLongestSubstring(string s)
    {
        if (s.Length <= 1)
            return s.Length;

        var longestLength = 1;
        var currentIndex = 0;
        for (var i = 1; i < s.Length; i++)
        {
            var currentLength = i - currentIndex;
            var index = s.LastIndexOf(s[i], i - 1, currentLength);
            if (index != -1)
            {
                currentIndex = index + 1;
                longestLength = System.Math.Max(longestLength, currentLength);
            }

            if (s.Length - currentIndex < longestLength)
                return longestLength;
        }

        return System.Math.Max(longestLength, s.Length - currentIndex);
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/longest-substring-without-repeating-characters/")]
    [NUnit.Framework.TestCase("abcabcbb", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("bbbbb", ExpectedResult = 1)]
    [NUnit.Framework.TestCase("pwwkew", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("", ExpectedResult = 0)]
    [NUnit.Framework.TestCase("a", ExpectedResult = 1)]
    [NUnit.Framework.TestCase("ab", ExpectedResult = 2)]
    [NUnit.Framework.TestCase("dvdf", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("anviaj", ExpectedResult = 5)]
    public object LengthOfLongestSubstring(params object[] args) => InvokeTest();
}
