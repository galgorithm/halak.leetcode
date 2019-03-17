using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Alvis
{
    internal static class Json
    {
        public static string Serialize(object value)
        {
            using (var underlyingWriter = new StringWriter(CultureInfo.InvariantCulture))
            using (var writer = new JsonTextWriter(underlyingWriter))
            {
                GetJsonSerializer().Serialize(writer, value);
                return underlyingWriter.ToString();
            }
        }

        public static object Deserizlie(string s, Type targetType)
        {
            using (var underlyingReader = new StringReader(s))
            using (var reader = new JsonTextReader(underlyingReader))
                return GetJsonSerializer().Deserialize(reader, targetType);
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
