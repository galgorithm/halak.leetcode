partial class Solution
{
    public string AddStrings(string num1, string num2)
    {
        var characters = new char[System.Math.Max(num1.Length, num2.Length) + 1];

        var carry = 0;
        var index = 0;
        for (; index < characters.Length - 1; index++)
        {
            var n1 = index < num1.Length ? (num1[num1.Length - index - 1] - '0') : 0;
            var n2 = index < num2.Length ? (num2[num2.Length - index - 1] - '0') : 0;
            carry += n1 + n2;
            characters[index] = (char)((carry % 10) + '0');
            carry /= 10;
        }

        if (carry != 0)
            characters[index++] = (char)(carry + '0');

        System.Array.Reverse(characters, 0, index);

        return new string(characters, 0, index);
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/add-strings/")]
    [NUnit.Framework.TestCase("1", "9", ExpectedResult = "10")]
    [NUnit.Framework.TestCase("7", "7", ExpectedResult = "14")]
    [NUnit.Framework.TestCase("91", "47", ExpectedResult = "138")]
    [NUnit.Framework.TestCase("594", "559", ExpectedResult = "1153")]
    [NUnit.Framework.TestCase("322", "686", ExpectedResult = "1008")]
    [NUnit.Framework.TestCase("4", "70", ExpectedResult = "74")]
    [NUnit.Framework.TestCase("49", "2", ExpectedResult = "51")]
    [NUnit.Framework.TestCase("43120", "7418483", ExpectedResult = "7461603")]
    [NUnit.Framework.TestCase("562641", "92862", ExpectedResult = "655503")]
    [NUnit.Framework.TestCase("7509360", "6", ExpectedResult = "7509366")]
    [NUnit.Framework.TestCase("1147", "786", ExpectedResult = "1933")]
    [NUnit.Framework.TestCase("81452360", "5", ExpectedResult = "81452365")]
    [NUnit.Framework.TestCase("35", "1090901359", ExpectedResult = "1090901394")]
    [NUnit.Framework.TestCase("15033", "42", ExpectedResult = "15075")]
    [NUnit.Framework.TestCase("45208675955053", "7278", ExpectedResult = "45208675962331")]
    [NUnit.Framework.TestCase("44551185", "74605367923", ExpectedResult = "74649919108")]
    [NUnit.Framework.TestCase("640335", "76", ExpectedResult = "640411")]
    [NUnit.Framework.TestCase("4", "7509787818", ExpectedResult = "7509787822")]
    [NUnit.Framework.TestCase("18900", "934518951856", ExpectedResult = "934518970756")]
    [NUnit.Framework.TestCase("691303600541819", "3501779957", ExpectedResult = "691307102321776")]
    [NUnit.Framework.TestCase("3", "1", ExpectedResult = "4")]
    [NUnit.Framework.TestCase("2", "9", ExpectedResult = "11")]
    [NUnit.Framework.TestCase("9969", "4963", ExpectedResult = "14932")]
    [NUnit.Framework.TestCase("7339", "9941", ExpectedResult = "17280")]
    [NUnit.Framework.TestCase("198261869", "893999312", ExpectedResult = "1092261181")]
    [NUnit.Framework.TestCase("702188531", "18721741", ExpectedResult = "720910272")]
    [NUnit.Framework.TestCase("8972259741739461", "2544536439154778", ExpectedResult = "11516796180894239")]
    [NUnit.Framework.TestCase("497154663256387706854192057061334893", "924495632044702262020419650281148488", ExpectedResult = "1421650295301089968874611707342483381")]
    public object AddStrings(params object[] args) => InvokeTest();

    public void AddStringsTestCase(int count)
    {
        var random = new System.Random();
        for (var i = 1; i <= count; i++)
        {
            var num1 = GenerateRandomNumber(random.Next(1, i));
            var num2 = GenerateRandomNumber(random.Next(1, i));
            PrintTestCaseCode(AddStrings(num1, num2), num1, num2);
        }

        string GenerateRandomNumber(int length)
        {
            var range = System.Linq.Enumerable.Range(0, length);
            var digits = System.Linq.Enumerable.Select(range, i => ((char)random.Next('0', '9' + 1)).ToString());
            return string.Join(string.Empty, digits).TrimStart('0');
        };

        string AddStrings(string num1, string num2)
            => (Parse(num1) + Parse(num2)).ToString(InvariantCulture);
    }
}
