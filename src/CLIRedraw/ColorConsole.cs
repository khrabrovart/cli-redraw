using System;

namespace CLIRedraw
{
    public static class ColorConsole
    {
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream
        /// using the specified background and foreground colors.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        public static void WriteLine(string text, ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Write(text, backgroundColor, foregroundColor);
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified string value to the standard output stream 
        /// using the specified background and foreground colors.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        public static void Write(string text, ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
