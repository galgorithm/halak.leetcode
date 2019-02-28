using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NUnit.Framework;

partial class Solution
{
    static void Draw(Action<IDebugPainter> handle) => DebugPainters.Draw(handle);
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

    static object Unordered1D<T>(IList<T> source)
    {
        return Internal.CreateEquatable(source, UnorderedEqual);

        bool UnorderedEqual(IList<T> left, IList<T> right)
        {
            if (left.Count != right.Count)
                return false;

            var comparer = Comparer<T>.Default;
            return left.OrderBy(it => it, comparer).SequenceEqual(
                right.OrderBy(it => it, comparer),
                EqualityComparer<T>.Default);
        }
    }

    static object Unordered2D<T>(IList<IList<T>> source)
    {
        return Internal.CreateEquatable(source, UnorderedEqual);

        bool UnorderedEqual(IList<IList<T>> left, IList<IList<T>> right)
        {
            if (left.Count != right.Count)
                return false;

            var comparer = Comparer<T>.Default;
            var equalityComparer = EqualityComparer<T>.Default;
            var x = Internal.Sorted(left, comparer);
            var y = Internal.Sorted(right, comparer);
            for (var i = 0; i < x.Length; i++)
            {
                if (x[i].SequenceEqual(y[i], equalityComparer) == false)
                    return false;
            }

            return true;
        }
    }

    static string[] ReadString(string name) => File.ReadAllLines(Path.Combine(ProjectPath, name));
    static int[] ReadInt32Array(string name) => ReadString(name).Select(Internal.ParseInt32).ToArray();
    static int[][] ReadInt32Array2D(string name) => ReadString(name).Select(s => Internal.SplitByComma(s).Select(Internal.ParseInt32).ToArray()).ToArray();

    private static class Internal
    {
        public static T[][] Sorted<T>(IList<IList<T>> source, IComparer<T> comparer)
        {
            comparer = comparer ?? Comparer<T>.Default;

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

        public static IEquatable<T> CreateEquatable<T>(T obj, Func<T, T, bool> comparer)
            => new EquatableObject<T>(obj, comparer);

        public static int ParseInt32(string s) => int.Parse(s, CultureInfo.InvariantCulture);
        public static string[] SplitByComma(string s) => s.Split(',', StringSplitOptions.RemoveEmptyEntries);

        private sealed class EquatableObject<T> : IEquatable<T>
        {
            private readonly T obj;
            private readonly Func<T, T, bool> comparer;

            public EquatableObject(T obj, Func<T, T, bool> comparer)
            {
                this.obj = obj;
                this.comparer = comparer;
            }

            public bool Equals(T other) => comparer.Invoke(obj, other);
            public override bool Equals(object obj) => obj is T other && Equals(other);
            public override int GetHashCode() => obj?.GetHashCode() ?? 0;
            public override string ToString() => obj?.ToString() ?? string.Empty;
        }
    }
}
