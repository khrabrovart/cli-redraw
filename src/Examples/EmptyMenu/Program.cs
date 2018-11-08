using CLIRedraw;
using System;

namespace EmptyMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu();

            menu.Add(new MenuItem("Add menu item", mi =>
            {
                menu.Add("New menu item", min =>
                {
                    Console.WriteLine("From new menu item");
                    Console.ReadKey();
                });
            }));

            menu.Show();

            Console.ReadKey();
        }
    }
}
