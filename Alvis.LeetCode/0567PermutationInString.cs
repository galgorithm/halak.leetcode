partial class Solution
{
    private static readonly int[] MagicCodes = new[]
    {
        0x5CF58A1A,
        0x1FD45F46,
        0x62B33471,
        0x2592099D,
        0x6870DEC8,
        0x2B4FB3F4,
        0x6E2E891F,
        0x310D5E4B,
        0x73EC3376,
        0x36CB08A2,
        0x79A9DDCD,
        0x3C88B2F9,
        0x7F678824,
        0x42465D50,
        0x525327C0,
        0x480407A7,
        0x0AE2DCD3,
        0x4DC1B1FE,
        0x10A0872A,
        0x537F5C55,
        0x165E3181,
        0x593D06AC,
        0x1C1BDBD8,
        0x5EFAB103,
        0x21D9862F,
        0x64B85B5A,
    };

    public bool CheckInclusion(string s1, string s2)
    {
        if (s2.Length < s1.Length)
            return false;

        const int HashCodeInitial = 0x2AB0F14E;

        var hashCode = HashCodeInitial;
        var accumulatedHashCode = HashCodeInitial;
        for (var i = 0; i < s1.Length; i++)
        {
            hashCode ^= GetMagicCode(s1[i]);
            accumulatedHashCode ^= GetMagicCode(s2[i]);
        }

        var sortedCharacters = new char[s1.Length];
        var temporaryCharacterSet = new System.Collections.Generic.List<char>(s1.Length);
        s1.CopyTo(0, sortedCharacters, 0, s1.Length);
        System.Array.Sort(sortedCharacters);

        for (var i = 0; i < s2.Length - s1.Length; i++)
        {
            if (hashCode == accumulatedHashCode && AreEqualUnordered(s2, i))
                return true;

            accumulatedHashCode ^= GetMagicCode(s2[i]);
            accumulatedHashCode ^= GetMagicCode(s2[i + s1.Length]);
        }

        return hashCode == accumulatedHashCode && AreEqualUnordered(s2, s2.Length - s1.Length);

        bool AreEqualUnordered(string s, int startIndex)
        {
            var characterSet = temporaryCharacterSet;
            for (var i = 0; i < sortedCharacters.Length; i++)
                temporaryCharacterSet.Add(sortedCharacters[i]);

            for (var i = 0; i < sortedCharacters.Length; i++)
            {
                var index = characterSet.BinarySearch(s[startIndex + i]);
                if (index >= 0)
                    characterSet.RemoveAt(index);
                else
                    return false;
            }

            return true;
        }

        int GetMagicCode(char c) => MagicCodes[c - 'a'];
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/permutation-in-string/")]
    [NUnit.Framework.TestCase("ab", "eidbaooo", ExpectedResult = true)]
    [NUnit.Framework.TestCase("ab", "eidboaoo", ExpectedResult = false)]
    [NUnit.Framework.TestCase("adc", "dcda", ExpectedResult = true)]
    public object CheckInclusion(params object[] args) => InvokeTest();
}
