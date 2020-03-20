using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace JsonCompare
{
    internal class JsonReader
    {
        public static bool TryReadFromFile(string path, bool keysToCamelCase, out Dictionary<string, string> dictionary)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is empty", nameof(path));
            dictionary = new Dictionary<string, string>();
            try
            {
                JToken token = JToken.Parse(File.ReadAllText(Path.GetFullPath(path)));
                FillDictionaryFromJToken(dictionary, token, keysToCamelCase, "");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        private static void FillDictionaryFromJToken(Dictionary<string, string> dict, JToken token, bool keysToCamelCase, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        FillDictionaryFromJToken(dict, prop.Value, keysToCamelCase,Join(prefix,keysToCamelCase? ToCamelCase(prop.Name) : prop.Name ));
                    }
                    break;

                case JTokenType.Array:
                    int index = 0;
                    foreach (JToken value in token.Children())
                    {
                        FillDictionaryFromJToken(dict, value, keysToCamelCase,Join(prefix, index.ToString()));
                        index++;
                    }
                    break;

                default:
                    dict.Add(prefix, Convert.ToString(((JValue)token).Value));
                    break;
            }
        }

        private static string ToCamelCase(string prefix)
        {
            if (Char.IsLower(prefix[0]))
                return prefix;
            return Char.ToLower(prefix[0]) + prefix.Substring(1);
        }

        private static string Join(string prefix, string name)
        {
            return (string.IsNullOrEmpty(prefix) ? name : prefix + "." + name);
        }
    }
}