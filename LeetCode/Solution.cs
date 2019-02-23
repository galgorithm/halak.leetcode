using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

partial class Solution
{
}

[TestFixture]
partial class Tests
{
    static string ProjectPath
    {
        get
        {
            var path = Environment.CurrentDirectory;
            while (Exists(path) == false)
                path = Path.GetDirectoryName(path);

            return path;

            bool Exists(string directoryPath)
            {
                if (string.IsNullOrEmpty(directoryPath))
                    throw new ArgumentNullException(nameof(directoryPath));

                var enumerator = Directory.EnumerateFiles(directoryPath, "*.csproj", SearchOption.TopDirectoryOnly).GetEnumerator();
                try
                {
                    return enumerator.MoveNext();
                }
                finally
                {
                    enumerator.Dispose();
                }
            }
        }
    }

    static object InvokeTest()
    {
        var currentTest = TestContext.CurrentContext?.Test;
        if (currentTest != null)
        {
            var solution = new Solution();
            var method = solution.GetType().GetMethod(currentTest.MethodName);
            return method.Invoke(solution, currentTest.Arguments);
        }

        throw new InvalidOperationException();
    }

    static T[][] Sorted<T>(IList<IList<T>> source)
    {
        var comparer = Comparer<T>.Default;
        var array = new T[source.Count][];
        for (var i = 0; i < source.Count; i++)
        {
            array[i] = source[i].ToArray();
            Array.Sort(array[i], comparer);
        }
        Array.Sort(array, (x, y) =>
        {
            if (x.Length != y.Length)
                return x.Length.CompareTo(y.Length);
            else
            {
                for (var i = 0; i < x.Length; i++)
                {
                    var result = comparer.Compare(x[i], y[i]);
                    if (result != 0)
                        return result;
                }

                return 0;
            }
        });
        return array;
    }

    static string[] ReadAllLines(string name) => File.ReadAllLines(Path.Combine(ProjectPath, name));
}
