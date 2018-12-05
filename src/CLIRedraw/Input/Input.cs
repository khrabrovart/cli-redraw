using System;

namespace CLIRedraw
{
    public class Input
    {
        public ConsoleColor TextBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor TextForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor InputBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor InputForegroundColor { get; set; } = ConsoleColor.White;

        public bool ClearBeforeInput { get; set; } = true;

        public string Show(string text)
        {
            if (ClearBeforeInput)
            {
                Console.Clear();
            }

            ColorConsole.Write($"{text}: ", TextBackgroundColor, TextForegroundColor);

            Console.BackgroundColor = InputBackgroundColor;
            Console.ForegroundColor = InputForegroundColor;

            var input = Console.ReadLine();

            Console.ResetColor();

            return input;
        }
    }
}
