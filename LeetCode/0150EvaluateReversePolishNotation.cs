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
    [NUnit.Framework.TestCaseSource(nameof(EvalRPNArgs))]
    public object EvalRPN(string[] tokens) => InvokeTest();

    static System.Collections.IEnumerable EvalRPNArgs
    {
        get
        {
            yield return new NUnit.Framework.TestCaseData((object)new[] { "2", "1", "+", "3", "*" }).Returns(9);
            yield return new NUnit.Framework.TestCaseData((object)new[] { "4", "13", "5", "/", "+" }).Returns(6);
            yield return new NUnit.Framework.TestCaseData((object)new[] { "10", "6", "9", "3", "+", "-11", "*", "/", "*", "17", "+", "5", "+" }).Returns(22);
        }
    }
}
