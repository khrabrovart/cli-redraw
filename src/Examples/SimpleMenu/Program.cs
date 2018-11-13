using System;
using CLIRedraw;

namespace SimpleMenu
{
    public class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu("Simple Menu")
            {
                ForegroundColor = ConsoleColor.DarkYellow
            };

            var sayHelloMenuAction = new MenuAction(SayHello)
            {
                IsCursorVisible = false
            };

            menu.Add(new MenuItem("Say Hello!", sayHelloMenuAction));
            menu.Add(new MenuItem("Exit", new MenuAction
            {
                IsTerminator = true
            }));

            menu.Show();
        }

        private static void SayHello()
        {
            Console.WriteLine("Hello, World!");
            Console.ReadKey();
        }

        private static void Sum()
        {
            if (int.TryParse(InputPrompt("Enter first number"), out var a) &&
                int.TryParse(InputPrompt("Enter second number"), out var b))
            {
                Console.WriteLine($"Sum is {a + b}");
            }
            else
            {
                ColorConsole.WriteLine("Invalid value!", foregroundColor: ConsoleColor.Red);
            }

            Console.ReadKey();
        }

        private static string InputPrompt(string promptText)
        {
            Console.Write($"{promptText}: ");
            return Console.ReadLine();
        }
    }
}
