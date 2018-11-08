using System;

namespace CLIRedraw
{
    public static class ColorConsole
    {
        public static void WriteLine(string text, ConsoleColor bg = ConsoleColor.Black, ConsoleColor fg = ConsoleColor.Gray)
        {
            Write(text, bg, fg);
            Console.WriteLine();
        }

        public static void Write(string text, ConsoleColor bg = ConsoleColor.Black, ConsoleColor fg = ConsoleColor.Gray)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
