partial class Solution
{
    public bool IsInterleave(string s1, string s2, string s3)
    {
        return s1.Length + s2.Length == s3.Length && Invoke(0, 0, new bool[s1.Length + 1, s2.Length + 1]);

        bool Invoke(int i, int k, bool[,] deadRouteCache)
        {
            while (i < s1.Length && k < s2.Length)
            {
                if (deadRouteCache[i, k])
                    return false;

                var c = s3[i + k];
                if (s1[i] == c && s2[k] == c)
                {
                    if (Invoke(i + 1, k, deadRouteCache) || Invoke(i, k + 1, deadRouteCache))
                        return true;
                    else
                    {
                        deadRouteCache[i + 1, k] = true;
                        deadRouteCache[i, k + 1] = true;
                        return false;
                    }
                }
                else if (s1[i] == c)
                    i++;
                else if (s2[k] == c)
                    k++;
                else
                    return false;
            }

            if (i < s1.Length)
                return string.CompareOrdinal(s1, i, s3, i + k, s1.Length - i) == 0;
            else if (k < s2.Length)
                return string.CompareOrdinal(s2, k, s3, i + k, s2.Length - k) == 0;
            else
                return true;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/interleaving-string/")]
    [NUnit.Framework.TestCase("aabcc", "dbbca", "aadbbcbcac", ExpectedResult = true)]
    [NUnit.Framework.TestCase("aabcc", "dbbca", "aadbbbaccc", ExpectedResult = false)]
    [NUnit.Framework.TestCase("a", "b", "a", ExpectedResult = false)]
    [NUnit.Framework.TestCase("aabc", "abad", "aabcbaad", ExpectedResult = false)]
    public object IsInterleave(params object[] args) => InvokeTest();
}
