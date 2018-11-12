using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class MenuItem
    {
        private IDictionary<ConsoleKey, MenuItemAction> _actions;

        public MenuItem(string title) : this(title, description: null)
        {
        }

        public MenuItem(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public MenuItem(string title, Action action)
            : this(title, new MenuItemAction(action))
        {
        }

        public MenuItem(string title, Action<MenuItem> action)
            : this(title, new MenuItemAction(action))
        {
        }

        public MenuItem(string title, MenuItemAction action)
            : this(title, null, action)
        {
        }

        public MenuItem(string title, string description, Action action)
            : this(title, description, new MenuItemAction(action))
        {
        }

        public MenuItem(string title, string description, Action<MenuItem> action)
            : this(title, description, new MenuItemAction(action))
        {
        }

        public MenuItem(string title, string description, MenuItemAction action)
        {
            Title = title;
            Description = description;

            _actions = new Dictionary<ConsoleKey, MenuItemAction>();

            if (action != null)
            {
                _actions.Add(ConsoleKey.Enter, action);
            }
        }

        public MenuItem(string title, IDictionary<ConsoleKey, Action> actions)
            : this(title, null, actions)
        { 
        }

        public MenuItem(string title, IDictionary<ConsoleKey, Action<MenuItem>> actions)
            : this(title, null, actions)
        {
        }

        public MenuItem(string title, IDictionary<ConsoleKey, MenuItemAction> actions)
            : this(title, null, actions)
        {
        }

        public MenuItem(string title, string description, IDictionary<ConsoleKey, Action> actions)
        {
            Title = title;
            Description = description;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => new MenuItemAction(a.Value))
                ?? new Dictionary<ConsoleKey, MenuItemAction>();
        }

        public MenuItem(string title, string description, IDictionary<ConsoleKey, Action<MenuItem>> actions)
        {
            Title = title;
            Description = description;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => new MenuItemAction(a.Value))
                ?? new Dictionary<ConsoleKey, MenuItemAction>();
        }

        public MenuItem(string title, string description, IDictionary<ConsoleKey, MenuItemAction> actions)
        {
            Title = title;
            Description = description;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => a.Value) 
                ?? new Dictionary<ConsoleKey, MenuItemAction>();
        }

        public string Title { get; }

        public string Description { get; }

        public void AddOrUpdateAction(ConsoleKey key, MenuItemAction action)
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

        public bool TryGetAction(ConsoleKey key, out MenuItemAction action)
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