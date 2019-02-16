using System;
using System.IO;
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

    static string[] ReadAllLines(string name) => File.ReadAllLines(Path.Combine(ProjectPath, name));
}
