using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Alvis
{
    public abstract class TestsBase
    {
        protected static IFormatProvider InvariantCulture => CultureInfo.InvariantCulture;

        protected static BigInteger Parse(string s)
            => BigInteger.Parse(s, NumberStyles.Integer, InvariantCulture);

        protected static void PrintTestCaseCode(object expectedResult, params object[] args)
        {
            var argsCode = string.Join(", ", args.Select(Literalize));
            var resultCode = Literalize(expectedResult);
            NUnit.Framework.TestContext.WriteLine($"[NUnit.Framework.TestCase({argsCode}, ExpectedResult = {resultCode})]");
        }

        private static string Literalize(object value)
        {
            switch (Type.GetTypeCode(value?.GetType()))
            {
                case TypeCode.Object: return Escape(Json.Serialize(value), '"');
                case TypeCode.Char: return Escape(value.ToString(), '\'');
                case TypeCode.String: return Escape(value.ToString(), '"');
                default: return Convert.ToString(value, InvariantCulture).ToLowerInvariant();
            }

            string Escape(string s, char quotes)
            {
                s = s.Replace(quotes.ToString(), "\\" + quotes.ToString());
                s = s.Replace("\\", "\\\\");
                s = s.Replace("\0", "\\0");
                s = s.Replace("\a", "\\a");
                s = s.Replace("\b", "\\b");
                s = s.Replace("\f", "\\f");
                s = s.Replace("\r", "\\r");
                s = s.Replace("\t", "\\t");
                s = s.Replace("\v", "\\v");
                return string.Concat(quotes, s, quotes);
            }
        }
    }
}
