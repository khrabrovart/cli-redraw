using System;

namespace CLIRedraw
{
    public class Input
    {
        public ConsoleColor TextBackgroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        public ConsoleColor TextForegroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        public ConsoleColor InputBackgroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        public ConsoleColor InputForegroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        public bool ClearBeforeInput { get; set; } = true;

        public bool ClearAfterInput { get; set; } = true;

        public string Show(string text)
        {
            if (ClearBeforeInput)
            {
                Console.Clear();
            }

            ColoredConsole.Write($"{text}: ", TextBackgroundColor, TextForegroundColor);
            var input = ColoredConsole.ReadLine(InputBackgroundColor, InputForegroundColor);

            if (ClearAfterInput)
            {
                Console.Clear();
            }

            return input;
        }
    }
}
