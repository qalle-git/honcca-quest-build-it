using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HonccaBuildingGame.Classes.Extra
{
    static class FileHandler
    {
        public static void AddFile(string fileName, List<int> fileLines)
        {
            using StreamWriter writer = new StreamWriter($"Maps/{fileName}.txt", false, Encoding.UTF8);

            for (int currentLineIndex = 0; currentLineIndex < fileLines.Count; currentLineIndex++)
            {
                string currentLine = fileLines[currentLineIndex].ToString();

                writer.Write($"{currentLine}\n");
            }
        }

        public static List<int> GetFile(string fileName)
        {
            List<int> fileLines = new List<int>();

            if (!File.Exists($"Maps/{fileName}.txt"))
                return fileLines;

            StreamReader readFile = new StreamReader($"Maps/{fileName}.txt");

            string line = readFile.ReadLine();

            while (line != null)
            {
                fileLines.Add(int.Parse(line));

                line = readFile.ReadLine();
            }

            readFile.Close();

            return fileLines;
        }
    }
}
