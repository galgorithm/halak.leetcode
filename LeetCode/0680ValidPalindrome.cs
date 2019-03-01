partial class Solution
{
    public bool ValidPalindrome(string s)
    {
        return Invoke(0, s.Length - 1);

        bool Invoke(int leftIndex, int rightIndex, int chance = 1)
        {
            while (leftIndex < rightIndex)
            {
                if (s[leftIndex] == s[rightIndex])
                {
                    leftIndex++;
                    rightIndex--;
                }
                else
                {
                    return chance > 0 && (
                        Invoke(leftIndex + 1, rightIndex, chance - 1) ||
                        Invoke(leftIndex, rightIndex - 1, chance - 1));
                }
            }

            return true;
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/valid-palindrome-ii/")]
    [NUnit.Framework.TestCase("aba", ExpectedResult = true)]
    [NUnit.Framework.TestCase("abca" , ExpectedResult = true)]
    [NUnit.Framework.TestCase("abccda" , ExpectedResult = false)]
    public object ValidPalindrome(params object[] args) => InvokeTest();
}
