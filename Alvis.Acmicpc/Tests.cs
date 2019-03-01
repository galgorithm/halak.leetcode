using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
partial class Tests
{
    static object InvokeTest()
    {
        var currentTest = TestContext.CurrentContext?.Test;
        if (currentTest == null)
            throw new InvalidOperationException();

        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod;
        var method = typeof(Tests).Assembly.GetType(currentTest.MethodName).GetMethod("Main", bindingFlags);

        var stdin = Console.In;
        var stdout = Console.Out;
        try
        {
            var nestedArguments = currentTest.Arguments[0] as object[];

            var input = string.Empty;
            using (var reader = new StringReader(nestedArguments[0].ToString().TrimStart()))
            {
                var lines = new System.Text.StringBuilder();
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    lines.AppendLine(line.Trim());
                input = lines.ToString();
            }

            using (var reader = new StringReader(input))
            using (var writer = new StringWriter())
            {
                Console.SetIn(reader);
                Console.SetOut(writer);

                global::VectorField.Main(new string[0]);

                return writer.ToString().TrimEnd();
            }
        }
        finally
        {
            Console.SetIn(stdin);
            Console.SetOut(stdout);
        }
    }
}
