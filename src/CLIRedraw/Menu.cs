using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class Menu
    {
        private int _currentIndex;
        private int _lastIndex;
        private List<MenuItem> _items;

        private string _title;
        private int _titleLinesCount;

        private int _currentBufferWidth;

        public Menu(IEnumerable<MenuItem> menuItems) 
            : this(null, menuItems)
        {
        }

        public Menu(string title, IEnumerable<MenuItem> menuItems)
        {
            _items = menuItems.ToList();
            _lastIndex = _items.Count - 1;

            _title = title;
            _titleLinesCount = GetLinesCount(title);

            _currentBufferWidth = Console.BufferWidth;
        }

        public MenuItem Current => _items[_currentIndex];

        public IEnumerable<MenuItem> Items => _items.ToList();

        public bool Looped { get; set; }

        public ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor SelectionBackgroundColor { get; set; } = ConsoleColor.DarkGray;

        public ConsoleColor SelectionForegroundColor { get; set; } = ConsoleColor.Green;

        public ConsoleColor TitleColor { get; set; } = ConsoleColor.Green;
        
        public void Show()
        {
            if (!_items.Any())
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
                        exit = Current.IsTerminator;
                        break;
                }
            }
        }

        public void Add(MenuItem menuItem)
        {
            _items.Add(menuItem);
            _lastIndex++;

            DrawItem(_lastIndex);
        }

        public void Remove(MenuItem menuItem)
        {
            if (Current == menuItem && _currentIndex == _lastIndex)
            {
                _currentIndex = _currentIndex - 1;
            }

            _items.Remove(menuItem);
            _lastIndex--;

            Redraw();
        }

        public void Remove(int index)
        {
            if (_currentIndex == index && _currentIndex == _lastIndex)
            {
                _currentIndex = _currentIndex - 1;
            }

            _items.RemoveAt(index);
            _lastIndex--;

            Redraw();
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
            _currentIndex = _currentIndex > 0 ? _currentIndex - 1 : _lastIndex;
            DrawItem(previousIndex);
            DrawItem(_currentIndex);
        }

        private void Down()
        {
            if (_currentIndex == _lastIndex && !Looped)
            {
                return;
            }

            if (CheckBufferWidthChanged())
            {
                Redraw();
            }

            var previousIndex = _currentIndex;
            _currentIndex = _currentIndex < _lastIndex ? _currentIndex + 1 : 0;
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

                action.Invoke(Current);

                if (Current.ClearBeforeAction && !Current.IsTerminator)
                {
                    Redraw();
                }
            }
        }

        private void Redraw()
        {
            Console.Clear();

            if (!string.IsNullOrWhiteSpace(_title))
            {
                ColorConsole.WriteLine(_title, fg: TitleColor);
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
                _titleLinesCount = GetLinesCount(_title);
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
