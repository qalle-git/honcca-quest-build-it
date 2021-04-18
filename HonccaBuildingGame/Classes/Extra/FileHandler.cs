using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HonccaBuildingGame.Classes.Extra
{
    static class FileHandler
    {
        /// <summary>
        /// Create a textfile with the lines included.
        /// </summary>
        /// <param name="fileName">Name of the text file.</param>
        /// <param name="fileLines">Each line that should exist inside the file.</param>
        public static void AddFile(string fileName, List<int> fileLines)
        {
            using StreamWriter writer = new StreamWriter($"Maps/{fileName}.txt", false, Encoding.UTF8);

            for (int currentLineIndex = 0; currentLineIndex < fileLines.Count; currentLineIndex++)
            {
                string currentLine = fileLines[currentLineIndex].ToString();

                writer.Write($"{currentLine}\n");
            }
        }

        /// <summary>
        /// Get a list of lines that exist inside the file.
        /// </summary>
        /// <param name="fileName">The files name.</param>
        /// <returns>A list with all the file lines.</returns>
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
