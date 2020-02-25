using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLIRedraw
{
    public class DirectoryTree
    {
        const string VerticalSign = "│ ";
        const string IntermediateNodeSign = "├─";
        const string TerminalNodeSign = "└─";

        public ConsoleColor FileNodeBackgroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        public ConsoleColor FileNodeForegroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        public ConsoleColor DirectoryNodeBackgroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        public ConsoleColor DirectoryNodeForegroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        public ConsoleColor LinesBackgroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        public ConsoleColor LinesForegroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        public void Show(string path)
        {
            PrintDirectoryTree(path);
        }

        private void PrintDirectoryTree(string path, int level = 0, List<int> intermediateLevels = null)
        {
            intermediateLevels = intermediateLevels ?? new List<int>();
            intermediateLevels.Add(level);

            if (level == 0)
            {
                ColoredConsole.WriteLine(path, DirectoryNodeBackgroundColor, DirectoryNodeForegroundColor);
            }

            if (!Directory.Exists(path))
            {
                return;
            }

            try
            {
                var files = Directory.GetFiles(path);
                var directories = Directory.GetDirectories(path);

                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = Path.GetFileName(files[i]);
                    var isTerminal = i == files.Length - 1 && directories.Length == 0;
                    var nodeSign = isTerminal ? TerminalNodeSign : IntermediateNodeSign;

                    ColoredConsole.Write($"{CreateIndent(level, intermediateLevels)}{nodeSign}", LinesBackgroundColor, LinesForegroundColor);
                    ColoredConsole.WriteLine(fileName, FileNodeBackgroundColor, FileNodeForegroundColor);
                }

                for (int i = 0; i < directories.Length; i++)
                {
                    var directoryName = Path.GetFileName(directories[i]);
                    var isTerminal = i == directories.Length - 1;
                    var nodeSign = isTerminal ? TerminalNodeSign : IntermediateNodeSign;

                    ColoredConsole.Write($"{CreateIndent(level, intermediateLevels)}{nodeSign}", LinesBackgroundColor, LinesForegroundColor);
                    ColoredConsole.WriteLine(directoryName, DirectoryNodeBackgroundColor, DirectoryNodeForegroundColor);

                    if (isTerminal)
                    {
                        intermediateLevels.Remove(level);
                    }

                    PrintDirectoryTree(directories[i], level + 1, intermediateLevels);
                }

                intermediateLevels.Remove(level);
            }
            catch (UnauthorizedAccessException)
            {
                // If access to directory is not allowed
            }
        }

        private string CreateIndent(int level, List<int> intermediateLevels)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < level; i++)
            {
                if (intermediateLevels.Contains(i))
                {
                    sb.Append(VerticalSign);
                }
                else
                {
                    sb.Append("   ");
                }
            }

            return sb.ToString();
        }
    }
}
