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

            using (var sourceReader = new StringReader(s))
            using (var reader = new JsonTextReader(sourceReader))
            {
                return GetJsonSerializer().Deserialize(reader, targetType);
            }
        }

        private static JsonSerializer serializer = null;
        private static JsonSerializer GetJsonSerializer()
        {
            return LazyInitializer.EnsureInitialized(ref serializer, () =>
            {
                return JsonSerializer.Create(new JsonSerializerSettings()
                {
                    StringEscapeHandling = StringEscapeHandling.Default,
                    FloatParseHandling = FloatParseHandling.Decimal,
                    FloatFormatHandling = FloatFormatHandling.DefaultValue,
                    DateParseHandling = DateParseHandling.None,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    Formatting = Formatting.None,
                    MaxDepth = 4,
                    DateFormatString = "O",
                    Culture = System.Globalization.CultureInfo.InvariantCulture,
                    Converters = new[]
                    {
                        new NaiveObjectConverter(),
                    },
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.None,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
            });
        }

        private sealed class NaiveObjectConverter : JsonConverter
        {
            private readonly ConcurrentDictionary<Type, ConstructorInfo> cachedConstructors;

            public override bool CanRead => true;
            public override bool CanWrite => false;

            public NaiveObjectConverter()
            {
                cachedConstructors = new ConcurrentDictionary<Type, ConstructorInfo>();
            }

            public override bool CanConvert(Type objectType) => GetConstructor(objectType) != null;
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var constructor = GetConstructor(objectType);
                var parameters = constructor.GetParameters();

                var args = new object[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    if (reader.Read())
                        args[i] = Convert.ChangeType(reader.Value, parameters[i].ParameterType);
                }

                if (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    throw new ArgumentException();

                return constructor.Invoke(args);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }

            private ConstructorInfo GetConstructor(Type objectType)
            {
                if (objectType.IsArray || objectType.IsAbstract || objectType.IsEnum || objectType.IsInterface)
                    return null;
                if (Type.GetTypeCode(objectType) != TypeCode.Object)
                    return null;

                return cachedConstructors.GetOrAdd(objectType, (type) =>
                {
                    var fields = objectType.GetFields(
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);
                    foreach (var constructor in objectType.GetConstructors())
                    {
                        var parameters = constructor.GetParameters();
                        if (parameters.Length == fields.Length)
                            return constructor;
                    }

                    return null;
                });
            }
        }
    }
}
