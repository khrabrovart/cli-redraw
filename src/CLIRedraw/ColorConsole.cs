using System;

namespace CLIRedraw
{
    public static class ColorConsole
    {
        public static void WriteLine(string text, ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Write(text, backgroundColor, foregroundColor);
            Console.WriteLine();
        }

        public static void Write(string text, ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
