using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class Menu
    {
        private List<MenuItem> _items;
        private MenuActionContext _invocationContext;
        private int _currentIndex;
        private int _titleLinesCount;
        private int _currentBufferWidth;
        private bool _isClosed;
        private bool _needRedraw;

        /// <summary>
        /// Represents menu control.
        /// </summary>
        public Menu() : this(null, null)
        {
        }

        /// <summary>
        /// Represents menu control with title.
        /// </summary>
        /// <param name="title">Menu title.</param>
        public Menu(string title) : this(title, null)
        {
        }

        /// <summary>
        /// Represents menu control with menu items.
        /// </summary>
        /// <param name="menuItems">Menu items collection.</param>
        public Menu(IEnumerable<MenuItem> menuItems) 
            : this(null, menuItems)
        {
        }

        /// <summary>
        /// Represents menu control with titile and menu items.
        /// </summary>
        /// <param name="title">Menu title.</param>
        /// <param name="menuItems">Menu items collection.</param>
        public Menu(string title, IEnumerable<MenuItem> menuItems)
        {
            _items = menuItems?.ToList() ?? new List<MenuItem>();
            _currentBufferWidth = Console.BufferWidth;

            if (title != null)
            {
                Title = title;
                _titleLinesCount = GetLinesCount(title);
            }
        }

        /// <summary>
        /// Gets currently selected menu item.
        /// </summary>
        public MenuItem CurrentItem => _items[_currentIndex];

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        public IEnumerable<MenuItem> Items => _items;

        /// <summary>
        /// Gets or sets the menu title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the menu looped state.
        /// </summary>
        public bool Looped { get; set; }

        /// <summary>
        /// Gets or sets background color for not selected menu items.
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        /// <summary>
        /// Gets or sets foreground color for not selected menu items.
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        /// <summary>
        /// Gets or sets background color for selected menu items.
        /// </summary>
        public ConsoleColor SelectedBackgroundColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        /// <summary>
        /// Gets or sets foreground color for selected menu items.
        /// </summary>
        public ConsoleColor SelectedForegroundColor { get; set; } = ColoredConsole.DefaultBackgroundColor;

        /// <summary>
        /// Gets or sets the menu title color.
        /// </summary>
        public ConsoleColor TitleColor { get; set; } = ColoredConsole.DefaultForegroundColor;

        private int LastIndex => _items.Count - 1;

        private bool IsEmpty => _items.Count == 0;

        private bool ShouldBeClosed => IsEmpty || _isClosed;

        private bool IsInvoking => _invocationContext != null;

        /// <summary>
        /// Shows the menu. Does nothing if the menu has no items.
        /// </summary>
        public void Show()
        {
            if (ShouldBeClosed)
            {
                return;
            }

            Redraw();

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
                        var action = GetAction(keyInfo.Key);

                        if (action != null)
                        {
                            InvokeAction(action);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void Close()
        {
            _isClosed = true;
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="menuItem">Menu item.</param>
        public void Add(MenuItem menuItem)
        {
            AddMenuItem(menuItem);
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <returns>Created menu item.</returns>
        public MenuItem Add(string title)
        {
            return AddMenuItem(new MenuItem(title));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        /// <returns>Created menu item.</returns>
        public MenuItem Add(string title, Action action)
        {
            return AddMenuItem(new MenuItem(title, new MenuAction(action)));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        /// <returns>Created menu item.</returns>
        public MenuItem Add(string title, Action<MenuActionContext> action)
        {
            return AddMenuItem(new MenuItem(title, action));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="actions">Menu item actions map.</param>
        /// <returns>Created menu item.</returns>
        public MenuItem Add(string title, IDictionary<ConsoleKey, Action> actions)
        {
            return AddMenuItem(new MenuItem(title, actions));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="actions">Menu item actions map.</param>
        /// <returns>Created menu item.</returns>
        public MenuItem Add(string title, IDictionary<ConsoleKey, Action<MenuActionContext>> actions)
        {
            return AddMenuItem(new MenuItem(title, actions));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        /// <returns>Created menu item.</returns>
        public MenuItem Add(string title, MenuAction action)
        {
            return AddMenuItem(new MenuItem(title, action));
        }

        /// <summary>
        /// Removes menu item from the menu.
        /// </summary>
        /// <param name="menuItem">Menu item to remove.</param>
        public void Remove(MenuItem menuItem)
        {
            if (CurrentItem == menuItem && _currentIndex == LastIndex)
            {
                _currentIndex--;
            }

            _items.Remove(menuItem);

            if (IsEmpty)
            {
                return;
            }

            if (!IsInvoking)
            {
                Redraw();
            }
            else
            {
                _needRedraw = true;
            }
        }

        /// <summary>
        /// Removes menu item from the menu.
        /// </summary>
        /// <param name="index">Index of the menu item to remove.</param>
        public void Remove(int index)
        {
            if (_currentIndex == index && _currentIndex == LastIndex)
            {
                _currentIndex--;
            }

            _items.RemoveAt(index);

            if (IsEmpty)
            {
                return;
            }

            if (!IsInvoking)
            {
                Redraw();
            }
            else
            {
                _needRedraw = true;
            }
        }

        private MenuItem AddMenuItem(MenuItem menuItem)
        {
            _items.Add(menuItem);

            if (!IsInvoking || !_invocationContext.MenuAction.ClearBeforeAction)
            {
                DrawItem(LastIndex);
            }
            else
            {
                _needRedraw = true;
            }

            return menuItem;
        }

        private void Up()
        {
            if (_currentIndex == 0 && !Looped)
            {
                return;
            }

            CheckBufferWidthChanged();

            var previousIndex = _currentIndex;
            _currentIndex = _currentIndex > 0 ? _currentIndex - 1 : LastIndex;
            DrawItem(previousIndex);
            DrawItem(_currentIndex);
        }

        private void Down()
        {
            if (_currentIndex == LastIndex && !Looped)
            {
                return;
            }

            CheckBufferWidthChanged();

            var previousIndex = _currentIndex;
            _currentIndex = _currentIndex < LastIndex ? _currentIndex + 1 : 0;
            DrawItem(previousIndex);
            DrawItem(_currentIndex);
        }

        private MenuAction GetAction(ConsoleKey key)
        {
            return CurrentItem.Actions.TryGetValue(key, out var action) ? action : null;
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

            if (menuAction.IsCursorVisible)
            {
                Console.CursorVisible = true;
            }

            InvokeInContext(menuAction);

            if ((menuAction.ClearBeforeAction || _needRedraw) && !ShouldBeClosed)
            {
                Redraw();
            }
        }

        private void InvokeInContext(MenuAction menuAction)
        {
            _invocationContext = new MenuActionContext(this, CurrentItem, menuAction);

            menuAction?.Action?.Invoke(_invocationContext);

            _invocationContext = null;
        }

        private void Redraw()
        {
            Console.Clear();

            if (!string.IsNullOrWhiteSpace(Title))
            {
                ColoredConsole.WriteLine(Title, foregroundColor: TitleColor);
                Console.WriteLine();
            }

            for (int i = 0; i < _items.Count; i++)
            {
                DrawItem(i);
            }
        }

        private void DrawItem(int index)
        {
            var isCurrent = _currentIndex == index;
            var menuItem = _items[index];

            if (menuItem == null)
            {
                return;
            }

            Console.SetCursorPosition(0, GetLinesCountToMenuItem(index));
            Console.CursorVisible = false;

            ColoredConsole.Write(menuItem.Title,
                isCurrent ? SelectedBackgroundColor : BackgroundColor,
                isCurrent ? SelectedForegroundColor : ForegroundColor);

            if (!string.IsNullOrWhiteSpace(menuItem.Description))
            {
                Console.WriteLine($" ({menuItem.Description})");
            }
            else
            {
                ColoredConsole.InvisibleWrite("-");
            }
        }

        private void CheckBufferWidthChanged()
        {
            if (Console.BufferWidth != _currentBufferWidth)
            {
                _currentBufferWidth = Console.BufferWidth;
                _titleLinesCount = GetLinesCount(Title);
            }
        }

        private int GetLinesCount(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            var lines = value.Split(new[] { "\n" }, StringSplitOptions.None);

            var linesCount = (int)lines.Sum(l => 
            {
                if (l.Length < Console.BufferWidth)
                {
                    return 1;
                }

                if (l.Length == Console.BufferWidth)
                {
                    return 2;
                }

                return Math.Ceiling((double)l.Length / Console.BufferWidth);
            });

            return linesCount;
        }

        private int GetLinesCountToMenuItem(int menuItemIndex)
        {
            var linesCount = _titleLinesCount + 1;

            for (int i = 0; i < menuItemIndex; i++)
            {
                linesCount += GetLinesCount(_items[i].ToString());
            }

            return linesCount;
        }
    }
}
