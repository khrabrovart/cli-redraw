using System;
using System.Collections.Generic;
using System.Threading;
using CLIRedraw;

namespace SimpleMenu
{
    public class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu()
            {
                Title = "The menu!\nAnother line",
                TitleBackgroundColor = ConsoleColor.Black,
                TitleForegroundColor = ConsoleColor.Green,
                Looped = true
            };

            menu.AddItem("Ask for hello", new ExplicitMenuAction(AskHello));

            var items = new List<MenuItem>();

            for (int i = 0; i < 200; i++)
            {
                items.Add(menu.AddItem("Test menu action we need to remove it", new ImplicitMenuAction(ImplicitAction)));
            }

            var bad = menu.AddItem("Say hello", new ExplicitMenuAction(() => SayHello("Default")));

            menu.AddItem("Remove item", new ImplicitMenuAction(() =>
            {
                menu.RemoveItem(bad);
            }));

            menu.AddItem("Remove all f* items", new ImplicitMenuAction(() =>
            {
                foreach (var item in items)
                {
                    menu.RemoveItem(item);
                }
            }));

            menu.AddItem("Exit", new ExplicitMenuAction(() => ExitPrompt(menu)));

            menu.Show();
        }

        private static void SayHello(string name)
        {
            Console.CursorVisible = false;
            Console.WriteLine($"Hello, {name}!");
            Console.ReadKey();
        }

        private static void AskHello()
        {
            Console.Write("Say hello to: ");
            var name = Console.ReadLine();

            SayHello(name);
        }

        private static void ImplicitAction()
        {
            Thread.Sleep(5000);
        }

        private static void ExitPrompt(Menu menu)
        {
            var prompt = new Menu();

            prompt.AddItem(new MenuItem("Yes", new ImplicitMenuAction(() => 
            {
                menu.Close();
                prompt.Close();
            })));

            prompt.AddItem(new MenuItem("No", new ImplicitMenuAction(() => prompt.Close())));

            prompt.Show();
        }
    }
}
