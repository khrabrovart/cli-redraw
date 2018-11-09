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

        public Menu() : this(null, null)
        {
        }

        public Menu(string title) : this(title, null)
        {
        }

        public Menu(IEnumerable<MenuItem> menuItems) 
            : this(null, menuItems)
        {
        }

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

        public string Title { get; set; }

        public MenuItem Current => _items[_currentIndex];

        public IEnumerable<MenuItem> Items => _items;

        public bool Looped { get; set; }

        public ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor SelectionBackgroundColor { get; set; } = ConsoleColor.DarkGray;

        public ConsoleColor SelectionForegroundColor { get; set; } = ConsoleColor.Green;

        public ConsoleColor TitleColor { get; set; } = ConsoleColor.Green;

        private int LastIndex => _items.Count - 1;

        private bool IsEmpty => _items.Count == 0;
        
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
                        InvokeAction(keyInfo.Key);
                        exit = _currentIndex < 0 || Current.IsTerminator;
                        break;
                }
            }
        }

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

        public void Add(string title)
        {
            Add(new MenuItem(title));
        }

        public void Add(string title, string description)
        {
            Add(new MenuItem(title, description));
        }

        public void Add(string title, Action<MenuItem> action)
        {
            Add(new MenuItem(title, action));
        }

        public void Add(string title, string description, Action<MenuItem> action)
        {
            Add(new MenuItem(title, description, action));
        }

        public void Add(string title, IDictionary<ConsoleKey, Action<MenuItem>> actions)
        {
            Add(new MenuItem(title, actions));
        }

        public void Add(string title, string description, IDictionary<ConsoleKey, Action<MenuItem>> actions)
        {
            Add(new MenuItem(title, description, actions));
        }

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

        private void InvokeAction(ConsoleKey key)
        {
            if (Current.TryGetAction(key, out var action))
            {
                if (Current.ClearBeforeAction)
                {
                    Console.Clear();
                }

                if (Current.ShowCursor)
                {
                    Console.CursorVisible = true;
                }

                _isInvoking = true;

                action.Invoke(Current);

                _isInvoking = false;

                if (IsEmpty)
                {
                    return;
                }

                if ((Current.ClearBeforeAction || _needRedraw) && !Current.IsTerminator)
                {
                    Redraw();
                }
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
                isCurrent ? SelectionBackgroundColor : DefaultBackgroundColor,
                isCurrent ? SelectionForegroundColor : DefaultForegroundColor);

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
