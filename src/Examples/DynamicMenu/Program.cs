using System;
using CLIRedraw;

namespace DynamicMenu
{
    class Program
    {
        private static int counter;

        public static void Main(string[] args)
        {
            var menu = new Menu("Dynamic Menu");

            var addMenuItem = menu.Add("Add menu item", ctx => AddMenuItem(ctx.Menu));
            addMenuItem.DefaultAction.ClearBeforeAction = false;

            menu.Add("Exit", context => context.Menu.Close());

            menu.Show();
        }

        private static void AddMenuItem(Menu menu)
        {
            var newMenuItem = new MenuItem($"New menu item {++counter}", "Enter - action; Delete - remove");

            var newMenuAction = new MenuAction
            {
                IsCursorVisible = false,
                Action = ctx =>
                {
                    Console.WriteLine("Menu item action...");
                    Console.ReadKey();
                }
            };

            newMenuItem.AddOrUpdateAction(ConsoleKey.Enter, newMenuAction);

            newMenuItem.AddOrUpdateAction(ConsoleKey.Delete, () =>
            {
                menu.Remove(newMenuItem);
            });

            menu.Add(newMenuItem);
        }
    }
}
