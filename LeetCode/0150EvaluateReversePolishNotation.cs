partial class Solution
{
    public int EvalRPN(string[] tokens)
    {
        var nfi = System.Globalization.NumberFormatInfo.InvariantInfo;
        var stack = new System.Collections.Generic.Stack<int>(tokens.Length);
        foreach (var token in tokens)
        {
            if (token.Length == 1)
            {
                switch (token[0])
                {
                    case '+': Evaluate((a, b) => a + b); break;
                    case '-': Evaluate((a, b) => a - b); break;
                    case '*': Evaluate((a, b) => a * b); break;
                    case '/': Evaluate((a, b) => a / b); break;
                    default: stack.Push(token[0] - '0'); break;
                }
            }
            else
                stack.Push(int.Parse(token, System.Globalization.NumberStyles.Integer, nfi));
        }

        return stack.Peek();

        void Evaluate(System.Func<int, int, int> op)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            stack.Push(op(left, right));
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/evaluate-reverse-polish-notation/")]
    [NUnit.Framework.TestCase(@"[""2"",""1"",""+"",""3"",""*""]", ExpectedResult = 9)]
    [NUnit.Framework.TestCase(@"[""4"",""13"",""5"",""/"",""+""]", ExpectedResult = 6)]
    [NUnit.Framework.TestCase(@"[""10"",""6"",""9"",""3"",""+"",""-11"",""*"",""/"",""*"",""17"",""+"",""5"",""+""]", ExpectedResult = 22)]
    public object EvalRPN(params object[] args) => InvokeTest();
}
