using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace JsonCompare
{
    internal class Comparator
    {
        private readonly Dictionary<string, string> _left;
        private readonly Dictionary<string, string> _right;
  
        public Comparator(Dictionary<string,string> left, Dictionary<string,string> right)
        {
             _left = left ?? throw new ArgumentNullException(nameof(left));
             _right = right ?? throw new ArgumentNullException(nameof(right));
        }


        public IEnumerable<CompareResult> GetMissedKeysOnTheRight(bool ignoreNulls)
        {
            return GetMissedItems(_left, _right, ignoreNulls);
        }

        public IEnumerable<CompareResult> GetMatchedByValue()
        {
            List<CompareResult> result = new List<CompareResult>();

            foreach (string key in _left.Keys)
            {
                if (_right.ContainsKey(key) && _left[key] == _right[key])
                {
                    result.Add(item: new CompareResult(key, _left[key], _right[key]));
                }
            }

            return result;
            
        }
        public IEnumerable<CompareResult> GetUnmatchedByValue()
        {
            List<CompareResult> result = new List<CompareResult>();

            foreach (string key in _left.Keys)
            {
                if (_right.ContainsKey(key) && _left[key] != _right[key])
                {
                    result.Add(item: new CompareResult(key, _left[key], _right[key]));
                }
            }

            return result;
        }

        public IEnumerable<CompareResult> GetMissedKeysOnTheLeft(bool ignoreNulls)
        {
            return GetMissedItems(_right, _left, ignoreNulls);
        }
        
        private static IEnumerable<CompareResult> GetMissedItems(Dictionary<string, string> items, 
            Dictionary<string, string> target, bool ignoreNulls)
        {
            List<CompareResult> result = new List<CompareResult>();
            
            foreach (string key in items.Keys)
            {
                if(ignoreNulls && string.IsNullOrEmpty(items[key]))
                    continue;

                if (!target.ContainsKey(key) )
                {
                    result.Add(item: new CompareResult(key, items[key]));
                }
            }

            return result;
        }

    }
}