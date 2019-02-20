using System;
using System.Collections.Generic;

partial class Solution
{
    public int RomanToInt(string s)
    {
        var n = 0;
        for (var i = 0; i < s.Length; i++)
        {
            var current = s[i];
            var next = i + 1 < s.Length ? s[i + 1] : '\0';
            switch (current)
            {
                case 'I':
                    switch (next) 
                    {
                        case 'V': n += 4; i++; break;
                        case 'X': n += 9; i++; break;
                        default: n += 1; break;
                    }
                    break;
                case 'V': 
                    n += 5;
                    break;
                case 'X':
                    switch (next) 
                    {
                        case 'L': n += 40; i++; break;
                        case 'C': n += 90; i++; break;
                        default: n += 10; break;
                    }
                    break;
                case 'L':
                    n += 50;
                    break;
                case 'C':
                    switch (next) 
                    {
                        case 'D': n += 400; i++; break;
                        case 'M': n += 900; i++; break;
                        default: n += 100; break;
                    }
                    break;
                case 'D':
                    n += 500;
                    break;
                case 'M':
                    n += 1000;
                    break;
            }
        }
        
        return n;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/roman-to-integer/")]
    [NUnit.Framework.TestCase("III", ExpectedResult = 3)]
    [NUnit.Framework.TestCase("IV", ExpectedResult = 4)]
    [NUnit.Framework.TestCase("IX", ExpectedResult = 9)]
    [NUnit.Framework.TestCase("LVIII", ExpectedResult = 58)]
    [NUnit.Framework.TestCase("MCMXCIV", ExpectedResult = 1994)]
    public int RomanToInt(string s) => new Solution().RomanToInt(s);
}
