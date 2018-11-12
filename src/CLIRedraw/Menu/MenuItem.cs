using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIRedraw
{
    public class MenuItem
    {
        private IDictionary<ConsoleKey, MenuItemAction> _actions;

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        public MenuItem(string title) : this(title, description: null)
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        public MenuItem(string title, string description)
        {
            Title = title;
            Description = description;

            _actions = new Dictionary<ConsoleKey, MenuItemAction>();
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        public MenuItem(string title, Action action)
            : this(title, new MenuItemAction(action))
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        public MenuItem(string title, Action<MenuItemActionContext> action)
            : this(title, new MenuItemAction(action))
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="action">Menu item default action.</param>
        public MenuItem(string title, MenuItemAction action)
            : this(title, null, action)
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="action">Menu item default action.</param>
        public MenuItem(string title, string description, Action action)
            : this(title, description, new MenuItemAction(action))
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="action">Menu item default action.</param>
        public MenuItem(string title, string description, Action<MenuItemActionContext> action)
            : this(title, description, new MenuItemAction(action))
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="action">Menu item default action.</param>
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

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, IDictionary<ConsoleKey, Action> actions)
            : this(title, null, actions)
        { 
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, IDictionary<ConsoleKey, Action<MenuItemActionContext>> actions)
            : this(title, null, actions)
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, IDictionary<ConsoleKey, MenuItemAction> actions)
            : this(title, null, actions)
        {
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, string description, IDictionary<ConsoleKey, Action> actions)
        {
            Title = title;
            Description = description;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => new MenuItemAction(a.Value))
                ?? new Dictionary<ConsoleKey, MenuItemAction>();
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, string description, IDictionary<ConsoleKey, Action<MenuItemActionContext>> actions)
        {
            Title = title;
            Description = description;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => new MenuItemAction(a.Value))
                ?? new Dictionary<ConsoleKey, MenuItemAction>();
        }

        /// <summary>
        /// Represents menu item.
        /// </summary>
        /// <param name="title">Menu item title.</param>
        /// <param name="description">Menu item description.</param>
        /// <param name="actions">Menu item actions map.</param>
        public MenuItem(string title, string description, IDictionary<ConsoleKey, MenuItemAction> actions)
        {
            Title = title;
            Description = description;

            _actions = actions?
                .Where(a => a.Value != null)
                .ToDictionary(a => a.Key, a => a.Value) 
                ?? new Dictionary<ConsoleKey, MenuItemAction>();
        }

        /// <summary>
        /// Gets the menu item title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the menu item description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Adds or updates the menu item action by its key.
        /// </summary>
        /// <param name="key">Action key.</param>
        /// <param name="action">Menu item action.</param>
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

        /// <summary>
        /// Adds or updates the menu item action by its key.
        /// </summary>
        /// <param name="key">Action key.</param>
        /// <param name="action">Menu item action.</param>
        public void AddOrUpdateAction(ConsoleKey key, Action<MenuItemActionContext> action)
        {
            AddOrUpdateAction(key, new MenuItemAction(action));
        }

        /// <summary>
        /// Adds or updates the menu item action by its key.
        /// </summary>
        /// <param name="key">Action key.</param>
        /// <param name="action">Menu item action.</param>
        public void AddOrUpdateAction(ConsoleKey key, Action action)
        {
            AddOrUpdateAction(key, new MenuItemAction(action));
        }

        /// <summary>
        /// Gets the menu item action by its key. 
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="key">Action key.</param>
        /// <param name="action">Menu item action.</param>
        /// <returns><see langword="true"/> if action exists; otherwise, <see langword="false"/>.</returns>
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