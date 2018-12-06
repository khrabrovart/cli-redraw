using System;
using CLIRedraw;

namespace SimpleMenu
{
    public class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu("Simple Menu Title");

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
            var myInput = new Input();

            int a;
            int b;

            while (!int.TryParse(myInput.Show("Enter first number"), out a) || !int.TryParse(myInput.Show("Enter second number"), out b))
            {
                ColoredConsole.WriteLine("Invalid input!", foregroundColor: ConsoleColor.Red);
                Console.ReadKey();
            }

            Console.WriteLine($"Sum is {a + b}");
            Console.ReadKey();
        }
    }
}
