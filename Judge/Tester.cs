﻿using System;
using System.IO;
using BashSoft.Exceptions;

namespace BashSoft
{
    public class Tester
    {
        public void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");

            try
            {
                string mismatchesPath = GetMismatchPath(expectedOutputPath);

                string[] actualOutputLines = File.ReadAllLines(userOutputPath);
                string[] expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                bool hasMismatch;
                string[] mismatches = GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputLines, out hasMismatch);

                PrintOutput(mismatches, hasMismatch, mismatchesPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            catch (FileNotFoundException)
            {
                throw new InvalidPathException();
            }
        }

        private void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchesPath)
        {
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                try
                {
                    File.WriteAllLines(mismatchesPath, mismatches);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new InvalidPathException();
                }

                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identicle. There are no mismatches");
        }

        private string[] GetLinesWithPossibleMismatches(string[] actualOutputLines, string[] expectedOutputLines, out bool hasMismatch)
        {
            hasMismatch = false;
            string output = string.Empty;

            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            int minOutputLines = actualOutputLines.Length;

            if (actualOutputLines.Length != expectedOutputLines.Length)
            {
                hasMismatch = true;
                minOutputLines = Math.Min(actualOutputLines.Length, expectedOutputLines.Length);
                OutputWriter.DisplayException(ExceptionMessages.ComparisonOfFilesWithDifferentSizes);
            }

            string[] mismatches = new string[minOutputLines];

            for (int index = 0; index < minOutputLines; index++)
            {
                string actualLine = actualOutputLines[index];
                string expectedLine = expectedOutputLines[index];

                if (!actualLine.Equals(expectedLine))
                {
                    output = $"Mismatch at line {index} -- expected: \"{expectedLine}\", actual: \"{actualLine}\"{Environment.NewLine}";
                    hasMismatch = true;
                }
                else
                {
                    output = $"{actualLine}{Environment.NewLine}";
                }

                mismatches[index] = output;
            }

            return mismatches;
        }

        private string GetMismatchPath(string expectedOutputPath)
        {
            int indexOfLastSlash = expectedOutputPath.LastIndexOf('\\');
            string directoryPath = expectedOutputPath.Substring(0, indexOfLastSlash);
            string finalPath = $"{directoryPath}\\Mismatches.txt";
            return finalPath;
        }
    }
}