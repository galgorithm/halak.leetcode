partial class Solution
{
    public int[] DiStringMatch(string S)
    {
        var min = 0;
        var max = S.Length;
        var array = new int[S.Length + 1];
        for (var i = 0; i < S.Length; i++)
            array[i] = (S[i] == 'I') ? (min++) : (max--);
        array[S.Length] = min;
        return array;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/di-string-match/")]
    [NUnit.Framework.TestCase("")]
    [NUnit.Framework.TestCase("I")]
    [NUnit.Framework.TestCase("D")]
    [NUnit.Framework.TestCase("IDID")]
    [NUnit.Framework.TestCase("III")]
    [NUnit.Framework.TestCase("DDD")]
    [NUnit.Framework.TestCase("DDII")]
    [NUnit.Framework.TestCase("IIDD")]
    [NUnit.Framework.TestCase("IIDDDIIIIIDDDDIDDIDIIDDIDDIDI")]
    [NUnit.Framework.TestCase("DIIIIDIDIIIDIIIDIDDIDIIDDII")]
    [NUnit.Framework.TestCase("DDIIIIDDDIDIIDIIIDIID")]
    [NUnit.Framework.TestCase("DDDIDDD")]
    [NUnit.Framework.TestCase("IIDIDIDDIIDIIDDIDDDDIDDIIDDI")]
    [NUnit.Framework.TestCase("IDDIIDDDI")]
    [NUnit.Framework.TestCase("IIDDIDIIIIIDIID")]
    [NUnit.Framework.TestCase("DIDDDDIIIIIDDDD")]
    [NUnit.Framework.TestCase("DIIDDDIIDDDIDIID")]
    public void DiStringMatch(string S)
    {
        var actual = new Solution().DiStringMatch(S);
        for (var i = 0; i < S.Length; i++)
        {
            if (S[i] == 'I')
                NUnit.Framework.Assert.Less(actual[i], actual[i + 1]);
            else
                NUnit.Framework.Assert.Greater(actual[i], actual[i + 1]);
        }

        System.Array.Sort(actual);
        for (var i = 0; i < actual.Length; i++)
            NUnit.Framework.Assert.AreEqual(actual[i], i);
    }
}
