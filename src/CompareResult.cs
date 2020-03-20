using System;

namespace JsonCompare
{
    internal struct CompareResult
    {
        public string Key { get; }
        public string Value { get; }
        public string Value2 { get; }

        public CompareResult(string key, string value)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value;
            Value2 = null;
        }

        public CompareResult(string key, string value, string value2)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value;
            Value2 = value2;
        }
    }
}