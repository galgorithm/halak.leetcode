partial class Solution
{
    public string ToLowerCase(string str)
    {
        // assert str match [A-Za-z0-9]+

        var index = 0;
        for (; index < str.Length; index++)
        {
            if ((str[index] & 32) == 0)
                break;
        }
        if (index == str.Length)
            return str;

        System.Span<char> array = stackalloc char[str.Length];
        for (var i = 0; i < index; i++)
            array[i] = str[i];
        for (var i = index; i < str.Length; i++)
            array[i] = (char)(str[i] | 32);

        return new string(array);
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/to-lower-case/")]
    [NUnit.Framework.TestCase("Hello", ExpectedResult = "hello")]
    [NUnit.Framework.TestCase("here", ExpectedResult = "here")]
    [NUnit.Framework.TestCase("LOVELY", ExpectedResult = "lovely")]
    public object ToLowerCase(string str) => InvokeTest();
}
