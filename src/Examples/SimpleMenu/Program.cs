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
                new MenuItem("Первый пункт"),
                new MenuItem("Второй пункт"),
                new MenuItem("Сумма чисел", mi => Sum()),
                new MenuItem("Четвертый пункт", "Совершает действие", mi => SomeAction1())
                {
                    ShowCursor = false
                },
                new MenuItem("Пятый пункт", "С выходом после действия", mi => 
                {
                    SomeAction2();
                    ConfirmExit(mi);
                })
                {
                    ShowCursor = false,
                    IsTerminator = true
                },
                new MenuItem("Выход", mi => ConfirmExit(mi))
            };

            var menu = new Menu("Добро пожаловать в меню!\nТестовый заголовок меню\nСостоит из нескольких строк", items)
            {
                DefaultForegroundColor = ConsoleColor.DarkYellow
            };

            var removeMeMenuItem = new MenuItem("Удали меня!", "Удаляет сам себя");
            removeMeMenuItem.AddOrUpdateAction(ConsoleKey.Enter, mi => menu.Remove(removeMeMenuItem));
            menu.Add(removeMeMenuItem);

            int i = 0;
            var addMeMenuItem = new MenuItem("Добавь еще!", "Добавляет новый пункт")
            {
                ShowCursor = false,
                ClearBeforeAction = false
            };

            addMeMenuItem.AddOrUpdateAction(ConsoleKey.Enter, mi => AddItem(menu, i++));
            menu.Add(addMeMenuItem);

            menu.Show();
        }

        public static void SomeAction1()
        {
            Console.WriteLine("Какое-то действие");
            Console.ReadKey();
        }

        public static void SomeAction2()
        {
            Console.WriteLine("После нажатия любой клавиши произойдет выход из программы");
            Console.ReadKey();
        }

        public static void AddItem(Menu menu, int index)
        {
            var menuItem = new MenuItem($"Новый пункт {index}", "Enter - действие, Delete - удалить", mi => SomeAction1());
            menuItem.AddOrUpdateAction(ConsoleKey.Delete, mi => menu.Remove(menuItem));

            menu.Add(menuItem);
        }

        public static void Sum()
        {
            if (int.TryParse(InputPrompt("Введите первое число"), out var a) &&
                int.TryParse(InputPrompt("Введите второе число"), out var b))
            {
                Console.WriteLine($"Сумма равна: {a + b}");
            }
            else
            {
                ColorConsole.WriteLine("Введено некорректное значение!", fg: ConsoleColor.Red);
            }

            Console.ReadKey();
        }

        public static void ConfirmExit(MenuItem item)
        {
            var yes = new MenuItem("Да", mi =>
                {
                    item.IsTerminator = true;
                });

            var no = new MenuItem("Нет", mi =>
                {
                    item.IsTerminator = false;
                });

            yes.IsTerminator = true;
            no.IsTerminator = true;

            new Menu("Вы точно хотите выйти?", new[] { yes, no }).Show();
        }

        private static string InputPrompt(string promptText)
        {
            Console.Write($"{promptText}: ");
            return Console.ReadLine();
        }
    }
}
