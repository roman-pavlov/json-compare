using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonCompare
{
    internal class CompareReportBuilder
    {
        private IEnumerable<CompareResult> _missedOnTheRight;
        private IEnumerable<CompareResult> _missedOnTheLeft;
        private IEnumerable<CompareResult> _unmatchedByValue;
        private IEnumerable<CompareResult> _matchedByValue;
        private const int KeyCellSize = 60;

        private List<string> _files = new List<string>();
        public CompareReportBuilder WithMissedOnTheRight(IEnumerable<CompareResult> missedOnTheRight)
        {
            _missedOnTheRight = missedOnTheRight;
            return this;
        }

        public CompareReportBuilder WithMissedOnTheLeft(IEnumerable<CompareResult> missedOnTheLeft)
        {
            _missedOnTheLeft = missedOnTheLeft;
            return this;
        }

        public CompareReportBuilder WithUnmatched(IEnumerable<CompareResult> unmatchedByValue)
        {
            _unmatchedByValue = unmatchedByValue;
            return this;
        }

        public CompareReportBuilder WithMatched(IEnumerable<CompareResult> matchedByValue)
        {
            _matchedByValue = matchedByValue;
            return this;
        }

        public string GetReport()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"JSON Comparison Report {DateTime.UtcNow.ToString("s")}");
            if (_files.Count > 0)
            {
                for (int i = 0; i < _files.Count; i++)
                {
                    sb.AppendLine($"File {i+1}: {_files[i]}");
                }
            }
            if (_missedOnTheRight != null)
            {
                sb.AppendLine($"{_missedOnTheRight.Count()} missed key-value pairs on the right (in File #2)");
                RenderItemsCol2(sb,_missedOnTheRight);
                sb.AppendLine();
            }
            
            if (_missedOnTheLeft != null)
            {
                sb.AppendLine($"{_missedOnTheLeft.Count()} missed key-value pairs on the left (in File #1)");
                RenderItemsCol2(sb,_missedOnTheLeft);
                sb.AppendLine();

            }
            if (_matchedByValue != null)
            {
                sb.AppendLine($"{_matchedByValue.Count()} matched Keys and Values!");
                RenderItemsCol2(sb,_matchedByValue);
                sb.AppendLine();
            }

            if (_unmatchedByValue != null)
            {
                sb.AppendLine($"{_unmatchedByValue.Count()} unmatched By Value (keys present but there are different values)");
                RenderItemsCol3(sb,_unmatchedByValue);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private void RenderItemsCol2(StringBuilder sb, IEnumerable<CompareResult> items)
        {
            sb.AppendLine($"------------------------------------------");
 
            
            string keyLine = null; 
            foreach (var item in items)
            {
                keyLine = $"        {item.Key}";
                int remainingSize = KeyCellSize >= keyLine.Length ? KeyCellSize - keyLine.Length : 0;
                sb.AppendLine($"{keyLine}{GetLineFilling(remainingSize)}|  {item.Value ?? "null"}  ");
            }
            sb.AppendLine($"------------------------------------------");
        }

        private string GetLineFilling( int remainingSize)
        {
            if (remainingSize <= 0) return string.Empty;
            
            var sb = new StringBuilder();
            for (int i = 0; i < remainingSize; i++)
            {
                sb.Append(' ');
            }

            return sb.ToString();
        }

        private void RenderItemsCol3(StringBuilder sb, IEnumerable<CompareResult> items)
        {
            sb.AppendLine($"------------------------------------------------------------------------------------");
            sb.AppendLine($"|        Key        |        Value (File #1)        |        Value (File #2)        |");
            sb.AppendLine($"------------------------------------------------------------------------------------");

            string keyLine = null;
            foreach (var item in items)
            {
                keyLine = $"        {item.Key}";
                int remainingSize = KeyCellSize >= keyLine.Length ? KeyCellSize - keyLine.Length : 0;
                sb.AppendLine($"{keyLine}{GetLineFilling(remainingSize)}|  {item.Value ?? "null"}  |  {item.Value2 ?? "null"}  ");          
            }
            sb.AppendLine($"---------------------------------------------------------------------");

        }

        public CompareReportBuilder WithFile(string fileName)
        {
            _files.Add(fileName);
            return this;
        }
    }
}