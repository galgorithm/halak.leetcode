using System;
using System.IO;

namespace Alvis
{
    public sealed class TestHelper
    {
        private static string projectPath = null;
        private static string GetProjectPath()
        {
            return System.Threading.LazyInitializer.EnsureInitialized(ref projectPath, () =>
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
            });
        }

        public static object SmartConvert(object obj, Type targetType)
        {
            var s = obj.ToString();
            if (s.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                var path = Path.Combine(GetProjectPath(), s);
                if (File.Exists(path))
                    s = File.ReadAllText(path);
            }
            else if (s.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                var path = Path.Combine(GetProjectPath(), s);
                if (File.Exists(path))
                {
                    var lines = File.ReadAllLines(path);
                    var elementType = targetType.GetElementType();
                    var array = Array.CreateInstance(elementType, lines.Length);
                    for (var i = 0; i < array.Length; i++)
                        array.SetValue(Convert.ChangeType(lines[i], elementType), i);
                    return array;
                }
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject(s, targetType);
        }
    }
}
