using System;
using System.Collections.Generic;
using System.IO;

namespace JsonCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments. There are at least two space-separated json files expected" +
                                  "\n You can set an optional output folder and an optional file suffix:" +
                                  "\n .\\JsonCompare.exe file1.json file2.json .\\out -suffix");
                return;
            }

            if (args.Length == 3 && !Directory.Exists(Path.GetFullPath(args[2])))
            {
                Console.WriteLine($"The output folder {args[2]} does not exist");
                return;
            }

            Dictionary<string, string>[] jsonFiles = new Dictionary<string, string>[2];

            for (int i = 0; i < 2; i++)
            {
                if (!IsJsonFileExist(args[i]))
                {
                    Console.WriteLine($"The file {args[i]} must be an existing json file");
                    return;
                }

                if (!JsonReader.TryReadFromFile(args[i], true, out jsonFiles[i]))
                {
                    Console.WriteLine($"Failed to read the file {args[i]}");
                    return;
                }
            }

            var comparator = new Comparator(jsonFiles[0], jsonFiles[1]);

            IEnumerable<CompareResult> missedOnTheRight = comparator.GetMissedKeysOnTheRight(true);
            IEnumerable<CompareResult> unmatchedByValue = comparator.GetUnmatchedByValue();
            IEnumerable<CompareResult> missedOnTheLeft = comparator.GetMissedKeysOnTheLeft(true);
            IEnumerable<CompareResult> matchedByValue = comparator.GetMatchedByValue();

            CompareReportBuilder rb = new CompareReportBuilder();

            rb.WithFile(args[0])
                .WithFile(args[1])
                .WithMissedOnTheRight(missedOnTheRight)
                .WithMissedOnTheLeft(missedOnTheLeft)
                .WithUnmatched(unmatchedByValue)
                .WithMatched(matchedByValue);

            string fileSuffix = args.Length >= 4 ? args[3] : "";
            string fileName = $"compare-" +
                              $"{DateTime.UtcNow.ToString("yyyy-M-d-HH-mm-ss")}" +
                              $"{fileSuffix}.txt";

            string path = args.Length >= 3 ? Path.GetFullPath(Path.Combine(args[2], fileName)) : fileName;
            File.WriteAllText(path, rb.GetReport());

            Console.WriteLine("Done.");
        }

        private static bool IsJsonFileExist(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                    return false;
                if (Path.GetExtension(file) != ".json")
                    return false;
                return File.Exists(Path.GetFullPath(file));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}