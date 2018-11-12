using System;
using System.Collections.Generic;
using System.Text;
using CLIRedraw;

namespace SimpleMenu
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var items = new List<MenuItem>
            {
                new MenuItem("First item"),
                new MenuItem("Second item"),
                new MenuItem("Sum", new MenuItemAction(Sum)),
                new MenuItem("Forth", "With action", mi => SomeAction1())
                {
                    ShowCursor = false
                },
                new MenuItem("Fifth", "With exit after action", mi => 
                {
                    SomeAction2();
                    ConfirmExit(mi);
                })
                {
                    ShowCursor = false,
                    IsTerminator = true
                },
                new MenuItem("Exit", mi => ConfirmExit(mi))
            };

            var menu = new Menu("Welcom to Simple Menu!\nSome menu title\nConsists of several text lines", items)
            {
                ForegroundColor = ConsoleColor.DarkYellow
            };

            var removeMeMenuItem = new MenuItem("Delete me!", "Removes itself");
            removeMeMenuItem.AddOrUpdateAction(ConsoleKey.Enter, mi => menu.Remove(removeMeMenuItem));
            menu.Add(removeMeMenuItem);

            int i = 0;
            var addMeMenuItem = new MenuItem("Add more!", "Adds new menu item")
            {
                ShowCursor = false,
                ClearBeforeAction = false
            };

            addMeMenuItem.AddOrUpdateAction(ConsoleKey.Enter, mi => AddItem(menu, i++));
            menu.Add(addMeMenuItem);

            menu.Show();
        }

        public static void SomeAction1(int? index = null)
        {
            Console.WriteLine("Some action");

            if (index.HasValue)
            {
                Console.WriteLine($"Index is {index}");
            }

            Console.ReadKey();
        }

        public static void SomeAction2()
        {
            Console.WriteLine("Application will exit after any key is pressed");
            Console.ReadKey();
        }

        public static void AddItem(Menu menu, int index)
        {
            var menuItem = new MenuItem($"New menu item {index}", "Enter or Delete", mi => SomeAction1(index));
            menuItem.AddOrUpdateAction(ConsoleKey.Delete, mi => menu.Remove(menuItem));

            menu.Add(menuItem);
        }

        public static void Sum()
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

        public static void ConfirmExit(MenuItem item)
        {
            var yes = new MenuItem("Yes", mi =>
                {
                    item.IsTerminator = true;
                });

            var no = new MenuItem("No", mi =>
                {
                    item.IsTerminator = false;
                });

            yes.IsTerminator = true;
            no.IsTerminator = true;

            new Menu("Are you sure you wanna quit?", new[] { yes, no }).Show();
        }

        private static string InputPrompt(string promptText)
        {
            Console.Write($"{promptText}: ");
            return Console.ReadLine();
        }
    }
}
