using System;
using System.Collections.Generic;
using System.Linq;

public class StreamChecker
{
    private readonly HashSet<string> words;
    private readonly HashSet<int> hashCodes;
    private readonly char[] buffer;
    private int bufferIndex;
    private readonly char[] temporaryBuffer;

    public StreamChecker(string[] words)
    {
        this.words = new HashSet<string>(words, StringComparer.Ordinal);
        this.hashCodes = words.Select(s => s.Reverse().Aggregate(0, (a, b) => HashCode.Combine(a, b))).ToHashSet();
        this.buffer = new char[2000];
        this.bufferIndex = 0;
        this.temporaryBuffer = new char[buffer.Length];
    }

    public bool Query(char letter)
    {
        buffer[bufferIndex] = letter;
        bufferIndex = (bufferIndex + 1) % buffer.Length;

        var hashCode = 0;
        var temporaryBufferIndex = temporaryBuffer.Length - 1;
        for (var i = Cycle(bufferIndex - 1); buffer[i] != '\0' && temporaryBufferIndex >= 0; i = Cycle(i - 1))
        {
            hashCode = HashCode.Combine(hashCode, buffer[i]);
            temporaryBuffer[temporaryBufferIndex] = buffer[i];

            if (hashCodes.Contains(hashCode))
            {
                var s = new string(temporaryBuffer, temporaryBufferIndex, temporaryBuffer.Length - temporaryBufferIndex);
                if (words.Contains(s))
                    return true;
            }

            temporaryBufferIndex--;
        }

        return false;

        int Cycle(int n) => (n + buffer.Length) % buffer.Length;
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/stream-of-characters/")]
    [NUnit.Framework.TestCase(new[] { "cd", "f", "kl" }, "abcdefghijkl", ExpectedResult = "FFFTFTFFFFFT")]
    public string StreamChecker(string[] words, string letter)
    {
        var checker = new StreamChecker(words);
        var result = new System.Text.StringBuilder(letter.Length);
        foreach (var c in letter)
            result.Append(checker.Query(c) ? 'T' : 'F');
        return result.ToString();
    }
}