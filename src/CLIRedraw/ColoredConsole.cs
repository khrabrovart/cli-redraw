using System;

namespace CLIRedraw
{
    public static class ColoredConsole
    {
        static ColoredConsole()
        {
            SaveDefaultColors();
        }

        public static ConsoleColor DefaultBackgroundColor { get; private set; }

        public static ConsoleColor DefaultForegroundColor { get; private set; }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream
        /// using the specified background and foreground colors.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        public static void WriteLine(string text, ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
        {
            Wrap(() => Console.WriteLine(text), backgroundColor, foregroundColor);
        }

        /// <summary>
        /// Writes the specified string value to the standard output stream 
        /// using the specified background and foreground colors.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        public static void Write(string text, ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
        {
            Wrap(() => Console.Write(text), backgroundColor, foregroundColor);
        }

        public static void InvisibleWrite(string text)
        {
            Write(text, DefaultBackgroundColor, DefaultBackgroundColor);
        }

        public static string ReadLine(ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
        {
            return Wrap(Console.ReadLine, backgroundColor, foregroundColor);
        }

        public static ConsoleKeyInfo ReadKey(ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
        {
            return Wrap(Console.ReadKey, backgroundColor, foregroundColor);
        }

        private static void Wrap(Action action, ConsoleColor? backgroundColor, ConsoleColor? foregroundColor)
        {
            if (action == null)
            {
                return;
            }

            Console.BackgroundColor = backgroundColor ?? DefaultBackgroundColor;
            Console.ForegroundColor = foregroundColor ?? DefaultForegroundColor;

            action();

            Console.ResetColor();
        }

        private static T Wrap<T>(Func<T> action, ConsoleColor? backgroundColor, ConsoleColor? foregroundColor)
        {
            if (action == null)
            {
                return default;
            }

            Console.BackgroundColor = backgroundColor ?? DefaultBackgroundColor;
            Console.ForegroundColor = foregroundColor ?? DefaultForegroundColor;

            var result = action();

            Console.ResetColor();

            return result;
        }

        private static void SaveDefaultColors()
        {
            var currentBgColor = Console.BackgroundColor;
            var currentFgColor = Console.ForegroundColor;

            Console.ResetColor();

            DefaultBackgroundColor = Console.BackgroundColor;
            DefaultForegroundColor = Console.ForegroundColor;

            Console.BackgroundColor = currentBgColor;
            Console.ForegroundColor = currentFgColor;
        }
    }
}
