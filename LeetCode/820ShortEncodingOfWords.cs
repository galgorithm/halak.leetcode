﻿using System;

partial class Solution
{
    public int MinimumLengthEncoding(string[] words)
    {
        Array.Sort(words, (a, b) => a.Length.CompareTo(b.Length));

        var wordMasks = new long[8];
        for (var i = 1; i < wordMasks.Length; i++)
            wordMasks[i] = (wordMasks[i - 1] << 8) | 0xFF;

        var wordCodes = new long[words.Length];
        for (var i = 0; i < words.Length; i++)
        {
            var wordCode = 0L;
            foreach (var c in words[i])
                wordCode = (wordCode << 8) | c;
            wordCodes[i] = wordCode;
        }

        var encodedLength = 0;
        for (var i = 0; i < words.Length; i++)
        {
            var wordCode = wordCodes[i];
            var wordMask = wordMasks[words[i].Length];
            var exists = false;
            for (var k = i + 1; k < wordCodes.Length; k++)
            {
                if ((wordCodes[k] & wordMask) == wordCode)
                {
                    exists = true;
                    break;
                }
            }

            if (exists == false)
                encodedLength += words[i].Length + 1;
        }

        return encodedLength;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/short-encoding-of-words/")]
    [NUnit.Framework.TestCaseSource(nameof(MinimumLengthEncodingArgs))]
    public object MinimumLengthEncoding(string[] words) => InvokeTest();

    static System.Collections.IEnumerable MinimumLengthEncodingArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[] { "time", "me", "bell" }).Returns(10);
            yield return new NUnit.Framework.TestCaseData((object)new[] { "time", "atime", "btime" }).Returns(12);
            yield return new NUnit.Framework.TestCaseData((object)ReadString("820ShortEncodingOfWords001.txt")).Returns(13964);
        }
    }
}
