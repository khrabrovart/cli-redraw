using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class Menu
    {
        private readonly List<MenuItem> _items;

        private MenuAction _runningAction;

        private int _selectedIndex;
        private bool _closed;
        private bool _showed;
        private int _startingPosition;

        private int? _redrawFromIndex;
        private int _removedItemsCount;

        private string _title;

        public Menu() : this(null, null)
        {
        }

        public Menu(string title) : this(title, null)
        {
        }

        public Menu(IEnumerable<MenuItem> menuItems) : this(null, menuItems) 
        {
        }

        public Menu(string title, IEnumerable<MenuItem> menuItems)
        {
            _items = menuItems?.ToList() ?? new List<MenuItem>();

            Title = title;
            BackgroundColor = ColoredConsole.DefaultBackgroundColor;
            ForegroundColor = ColoredConsole.DefaultForegroundColor;
            SelectedBackgroundColor = ColoredConsole.DefaultForegroundColor;
            SelectedForegroundColor = ColoredConsole.DefaultBackgroundColor;
        }

        public string Title 
        { 
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                _startingPosition = value.Split(new[] { "\n" }, StringSplitOptions.None).Length;
            }
        }

        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor SelectedBackgroundColor { get; set; }

        public ConsoleColor SelectedForegroundColor { get; set; }

        public ConsoleColor TitleBackgroundColor { get; set; }

        public ConsoleColor TitleForegroundColor { get; set; }

        public bool Looped { get; set; }

        public MenuItem SelectedItem => _items[_selectedIndex];

        private int LastIndex => _items.Count - 1;

        private bool ShouldBeClosed => _closed || !_items.Any();

        private bool IsInvoking => _runningAction != null;

        public void AddItem(MenuItem menuItem)
        {
            AddMenuItem(menuItem);
        }

        public MenuItem AddItem(string title, MenuAction action)
        {
            return AddMenuItem(new MenuItem(title, action));
        }

        public void RemoveItem(MenuItem menuItem)
        {
            var menuItemIndex = _items.IndexOf(menuItem);

            if (menuItemIndex == -1)
            {
                return;
            }

            if (SelectedItem == menuItem && _selectedIndex == LastIndex || menuItemIndex < _selectedIndex)
            {
                _selectedIndex--;
            }

            _items.Remove(menuItem);

            if (!_items.Any())
            {
                return;
            }

            if (_showed && !IsInvoking)
            {
                DrawMenu();
            }
            else if (IsInvoking && menuItemIndex >= 0)
            {
                _redrawFromIndex = menuItemIndex;
                _removedItemsCount++;
            }
        }

        public void Show()
        {
            if (ShouldBeClosed)
            {
                return;
            }

            DrawMenu();

            _showed = true;

            while (!ShouldBeClosed)
            {
                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        Up();
                        break;

                    case ConsoleKey.DownArrow:
                        Down();
                        break;

                    default:
                        if (SelectedItem.Actions.TryGetValue(keyInfo.Key, out var action))
                        {
                            InvokeAction(action);
                        }

                        break;
                }
            }
        }

        public void Close()
        {
            _closed = true;
        }

        private void DrawMenu(int? fromIndex = null)
        {
            Console.CursorVisible = false; // проверить, где лучше это расположить

            if (!string.IsNullOrWhiteSpace(_title) && fromIndex == null)
            {
                ColoredConsole.WriteLine(_title, TitleBackgroundColor, TitleForegroundColor);
            }

            for (int i = fromIndex ?? 0; i < _items.Count; i++)
            {
                DrawItem(i);
            }

            if (_removedItemsCount > 0)
            {
                for (int i = 0; i < _removedItemsCount; i++)
                {
                    DrawEmpty(_items.Count + i);
                }

                _removedItemsCount = 0;
            }

            Console.SetCursorPosition(0, _startingPosition + Math.Max(_selectedIndex - 1, 0));
            _redrawFromIndex = null;
        }

        private void DrawItem(int index)
        {
            var menuItem = _items[index];

            if (menuItem == null)
            {
                return;
            }

            Console.SetCursorPosition(0, _startingPosition + index);

            var title = menuItem.Title.PadRight(Console.BufferWidth, ' ');
            var selected = _selectedIndex == index;

            ColoredConsole.Write(
                title, 
                selected ? SelectedBackgroundColor : BackgroundColor, 
                selected ? SelectedForegroundColor : ForegroundColor);
        }

        private void DrawEmpty(int index)
        {
            Console.SetCursorPosition(0, _startingPosition + index);
            Console.Write(new string(' ', Console.BufferWidth));
        }

        private MenuItem AddMenuItem(MenuItem menuItem)
        {
            _items.Add(menuItem);

            if (_showed && !IsInvoking)
            {
                DrawItem(LastIndex);
            }

            return menuItem;
        }

        private void Up()
        {
            if (_selectedIndex == 0 && !Looped)
            {
                return;
            }

            var previousIndex = _selectedIndex;
            _selectedIndex = _selectedIndex > 0 ? _selectedIndex - 1 : LastIndex;
            DrawItem(previousIndex);
            DrawItem(_selectedIndex);
        }

        private void Down()
        {
            if (_selectedIndex == LastIndex && !Looped)
            {
                return;
            }

            var previousSelectedIndex = _selectedIndex;
            _selectedIndex = _selectedIndex < LastIndex ? _selectedIndex + 1 : 0;
            DrawItem(previousSelectedIndex);
            DrawItem(_selectedIndex);
        }

        private void InvokeAction(MenuAction menuAction)
        {
            if (menuAction?.Action == null)
            {
                return;
            }

            if (menuAction.ClearBeforeAction)
            {
                Console.Clear();
            }

            if (menuAction.ShowCursor)
            {
                Console.CursorVisible = true; // где мы его обратно скрываем?
            }

            _runningAction = menuAction;

            menuAction.Action.Invoke();

            _runningAction = null;

            if (!ShouldBeClosed)
            {
                if (menuAction.ClearBeforeAction)
                {
                    Console.Clear();
                    DrawMenu();
                }

                if (_redrawFromIndex != null)
                {
                    DrawMenu(_redrawFromIndex);
                }
            }
        }
    }
}
