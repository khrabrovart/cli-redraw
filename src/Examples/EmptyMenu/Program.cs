using CLIRedraw;
using System;
using System.Collections.Generic;

namespace EmptyMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu();

            var addMenuItem = new MenuItem("Add menu item", mi =>
            {
                menu.Add("New menu item", new Dictionary<ConsoleKey, Action<MenuItem>>
                {
                    [ConsoleKey.Delete] = min =>
                    {
                        menu.Remove(min);
                    }
                });
            });

            addMenuItem.AddOrUpdateAction(ConsoleKey.Delete, mi => menu.Remove(mi));

            menu.Add(addMenuItem);
            menu.Show();

            Console.ReadKey();
        }
    }
}
