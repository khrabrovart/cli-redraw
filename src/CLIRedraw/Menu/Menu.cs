using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class Menu
    {
        private int _currentIndex;
        private List<MenuItem> _items;
        private bool _isInvoking;
        private int _titleLinesCount;
        private int _currentBufferWidth;
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
        public MenuItem Current => _items[_currentIndex];

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
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets foreground color for not selected menu items.
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets background color for selected menu items.
        /// </summary>
        public ConsoleColor SelectedBackgroundColor { get; set; } = ConsoleColor.DarkGray;

        /// <summary>
        /// Gets or sets foreground color for selected menu items.
        /// </summary>
        public ConsoleColor SelectedForegroundColor { get; set; } = ConsoleColor.Green;

        /// <summary>
        /// Gets or sets the menu title color.
        /// </summary>
        public ConsoleColor TitleColor { get; set; } = ConsoleColor.Green;

        private int LastIndex => _items.Count - 1;

        private bool IsEmpty => _items.Count == 0;
        
        /// <summary>
        /// Shows the menu. Does nothing if the menu has no items.
        /// </summary>
        public void Show()
        {
            if (IsEmpty)
            {
                return;
            }

            var exit = false;
            Redraw();

            while (!exit)
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
                            exit = _currentIndex < 0 || action.IsTerminator;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="menuItem">Menu item.</param>
        public void Add(MenuItem menuItem)
        {
            _items.Add(menuItem);

            if (!_isInvoking)
            {
                DrawItem(LastIndex);
            }
            else
            {
                _needRedraw = true;
            }
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        public void Add(string title)
        {
            Add(new MenuItem(title));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        public void Add(string title, string description)
        {
            Add(new MenuItem(title, description));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        public void Add(string title, Action action)
        {
            Add(new MenuItem(title, new MenuAction(action)));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="action">Menu item default action.</param>
        public void Add(string title, string description, Action action)
        {
            Add(new MenuItem(title, description, new MenuAction(action)));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="actions">Menu item actions map.</param>
        public void Add(string title, IDictionary<ConsoleKey, Action> actions)
        {
            Add(new MenuItem(title, actions));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="actions">Menu item actions map.</param>
        public void Add(string title, string description, IDictionary<ConsoleKey, Action> actions)
        {
            Add(new MenuItem(title, description, actions));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        public void Add(string title, MenuAction action)
        {
            Add(new MenuItem(title, action));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="action">Menu item default action.</param>
        public void Add(string title, string description, MenuAction action)
        {
            Add(new MenuItem(title, description, action));
        }

        /// <summary>
        /// Adds new menu item to the menu.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        public void Add(string title, Action<MenuActionContext> action)
        {
            Add(new MenuItem(title, action));
        }

        /// <summary>
        /// Removes menu item from the menu.
        /// </summary>
        /// <param name="menuItem">Menu item to remove.</param>
        public void Remove(MenuItem menuItem)
        {
            if (Current == menuItem && _currentIndex == LastIndex)
            {
                _currentIndex--;
            }

            _items.Remove(menuItem);

            if (IsEmpty)
            {
                return;
            }

            if (!_isInvoking)
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

            if (!_isInvoking)
            {
                Redraw();
            }
            else
            {
                _needRedraw = true;
            }
        }

        private void Up()
        {
            if (_currentIndex == 0 && !Looped)
            {
                return;
            }

            if (CheckBufferWidthChanged())
            {
                Redraw();
            }

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

            if (CheckBufferWidthChanged())
            {
                Redraw();
            }

            var previousIndex = _currentIndex;
            _currentIndex = _currentIndex < LastIndex ? _currentIndex + 1 : 0;
            DrawItem(previousIndex);
            DrawItem(_currentIndex);
        }

        private MenuAction GetAction(ConsoleKey key)
        {
            return Current.TryGetAction(key, out var action) ? action : null;
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

            var context = new MenuActionContext(this, Current, menuAction);

            _isInvoking = true;

            menuAction.Action.Invoke(context);

            _isInvoking = false;

            if (IsEmpty)
            {
                return;
            }

            if ((menuAction.ClearBeforeAction || _needRedraw) && !menuAction.IsTerminator)
            {
                Redraw();
            }
        }

        private void Redraw()
        {
            Console.Clear();

            if (!string.IsNullOrWhiteSpace(Title))
            {
                ColorConsole.WriteLine(Title, foregroundColor: TitleColor);
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

            Console.SetCursorPosition(0, GetLinesCountToIndex(index));
            Console.CursorVisible = false;

            ColorConsole.Write(menuItem.Title,
                isCurrent ? SelectedBackgroundColor : BackgroundColor,
                isCurrent ? SelectedForegroundColor : ForegroundColor);

            if (!string.IsNullOrWhiteSpace(menuItem.Description))
            {
                ColorConsole.WriteLine($" ({menuItem.Description})");
            }
            else
            {
                Console.WriteLine();
            }
        }

        private bool CheckBufferWidthChanged()
        {
            var changed = Console.BufferWidth != _currentBufferWidth;

            if (changed)
            {
                _currentBufferWidth = Console.BufferWidth;
                _titleLinesCount = GetLinesCount(Title);
            }

            return changed;
        }

        private int GetLinesCount(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            var lines = value.Split(new[] { "\n" }, StringSplitOptions.None);

            return (int)lines.Sum(l => 
            {
                return Math.Ceiling((double)l.Length / Console.BufferWidth);
            });
        }

        private int GetLinesCountToIndex(int index)
        {
            var linesCount = _titleLinesCount;

            for (int i = 0; i < index; i++)
            {
                linesCount += GetLinesCount(_items[i].ToString());
            }

            return linesCount;
        }
    }
}
