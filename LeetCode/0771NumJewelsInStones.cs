partial class Solution
{
    public int NumJewelsInStones(string J, string S)
    {
        var jewelMask = 0L;
        for (var i = 0; i < J.Length; i++)
            jewelMask |= 1L << ToIndex(J[i]);
        
        var count = 0;
        for (var i = 0; i < S.Length; i++)
        {
            if ((jewelMask & (1L << ToIndex(S[i]))) != 0)
                count++;
        }

        return count;
        
        int ToIndex(char c) 
        {
            if ('A' <= c && c <= 'Z')
                return c - 'A';
            else if ('a' <= c && c <= 'z')
                return c - 'a' + 26;
            else
                return c - '0' + 52;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/jewels-and-stones/")]
    [NUnit.Framework.TestCase("aA", "aAAbbbb", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("z", "ZZ", ExpectedResult = 0)]
    public object NumJewelsInStones(string J, string S) => InvokeTest();
}
