using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;

namespace Alvis
{
    public sealed class TestHelper
    {
        private static string projectPath = null;
        private static string GetProjectPath()
        {
            return LazyInitializer.EnsureInitialized(ref projectPath, () =>
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
            else
            {
                // JSON 예외 상황
                // - C# 문자 리터럴 @"\"을 JSON에서 인식할 수 있도록 @"\\"로 변경합니다.
                // - JSON 문자열의 따옴표 문자는 '와 " 모두 허용합니다. 이 부분은 Json.NET에 기본으로 내장되어 있습니다.
                s = s.Replace(@"\", @"\\");
            }

            return Json.Deserizlie(s, targetType);
        }

    }
}
