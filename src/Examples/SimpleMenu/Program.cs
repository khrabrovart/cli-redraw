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
            var regexInput = new Int32Input("Enter first number")
            {
                ClearBeforeInput = false
            };

            var int32Input = new Int32Input("Enter second number")
            {
                ClearBeforeInput = false
            };

            var a = regexInput.Show();
            var b = int32Input.Show();

            Console.WriteLine($"Sum is {a + b}");
            Console.ReadKey();
        }
    }
}
