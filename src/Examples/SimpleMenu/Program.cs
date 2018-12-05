using System;
using System.Drawing;
using CLIRedraw;

namespace SimpleMenu
{
    public class Program
    {
        private static Point _point;

        static void Main(string[] args)
        {
            var menu = new Menu("Simple Menu")
            {
                ForegroundColor = ConsoleColor.DarkGray
            };

            var sayHelloMenuItem = menu.Add("Say Hello!", SayHello);
            sayHelloMenuItem.DefaultAction.IsCursorVisible = false;
            sayHelloMenuItem.Description = "Print Hello string";

            var calcSumMenuItem = menu.Add("Calc sum", Sum);
            calcSumMenuItem.Description = "Calculates sum of numbers";

            var exitMenuItem = menu.Add("Exit", context => context.Menu.Close());

            menu.Show();
        }

        private static void SayHello()
        {
            Console.WriteLine("Hello, World!");
            Console.ReadKey();
        }

        private static void Sum()
        {
            var myInput = new Input
            {
                TextForegroundColor = ConsoleColor.Yellow,
                InputForegroundColor = ConsoleColor.Green
            };

            int a;
            int b;

            while (!int.TryParse(myInput.Show("Enter first number"), out a) || !int.TryParse(myInput.Show("Enter second number"), out b))
            {
                ColorConsole.WriteLine("Invalid input!", foregroundColor: ConsoleColor.Red);
                Console.ReadKey();
            }

            Console.WriteLine($"Sum is {a + b}");
            Console.ReadKey();
        }
    }
}
