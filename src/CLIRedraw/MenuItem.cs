using System;
using System.Collections.Generic;

namespace CLIRedraw
{
    public class MenuItem
    {
        private IDictionary<ConsoleKey, Action<MenuItem>> _actions;

        public MenuItem(string title)
            : this(title, null, null)
        {
        }

        public MenuItem(string title, Action<MenuItem> action)
            : this(title, null, action)
        {
        }

        public MenuItem(string title, string description)
            : this(title, description, null)
        {
        }

        public MenuItem(string title, string description, Action<MenuItem> action)
        {
            Title = title;
            Description = description;

            _actions = new Dictionary<ConsoleKey, Action<MenuItem>>();

            if (action != null)
            {
                _actions.Add(ConsoleKey.Enter, action);
            }
        }

        public string Title { get; }

        public string Description { get; }

        public bool ClearBeforeAction { get; set; } = true;

        public bool ShowCursor { get; set; } = true;

        public bool IsTerminator { get; set; }

        public void AddOrUpdateAction(ConsoleKey key, Action<MenuItem> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (_actions.ContainsKey(key))
            {
                _actions[key] = action;
            }
            else
            {
                _actions.Add(key, action);
            }
        }

        public bool TryGetAction(ConsoleKey key, out Action<MenuItem> action)
        {
            if (_actions.TryGetValue(key, out var result))
            {
                action = result;
                return true;
            }

            action = null;
            return false;
        }

        public override string ToString()
        {
            return Title + (string.IsNullOrWhiteSpace(Description) ? null : $" ({Description})");
        }
    }
}