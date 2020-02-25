using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class MenuItem
    {
        private IDictionary<ConsoleKey, MenuAction> _actions;

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        public MenuItem(string title)
        {
            Title = title;

            _actions = new Dictionary<ConsoleKey, MenuAction>();
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="action">Menu item default action.</param>
        public MenuItem(string title, MenuAction action)
        {
            Title = title;

            _actions = new Dictionary<ConsoleKey, MenuAction>();

            if (action != null)
            {
                _actions.Add(ConsoleKey.Enter, action);
            }
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, IDictionary<ConsoleKey, MenuAction> actions)
        {
            Title = title;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => a.Value) 
                ?? new Dictionary<ConsoleKey, MenuAction>();
        }

        /// <summary>
        /// Gets the menu item title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the menu item actions.
        /// </summary>
        public IReadOnlyDictionary<ConsoleKey, MenuAction> Actions => (IReadOnlyDictionary<ConsoleKey, MenuAction>)_actions;

        /// <summary>
        /// Gets the default menu item action (Enter key).
        /// </summary>
        public MenuAction DefaultAction => _actions.TryGetValue(ConsoleKey.Enter, out var action) ? action : null;

        /// <summary>
        /// Adds or updates the menu item action by its key.
        /// </summary>
        /// <param name="key">Action key.</param>
        /// <param name="action">Menu item action.</param>
        public void AddOrUpdateAction(ConsoleKey key, MenuAction action)
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
    }
}