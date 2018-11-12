using System;

namespace CLIRedraw
{
    public class MenuItemAction
    {
        /// <summary>
        /// Represents empty menu item action.
        /// This constructor is commonly used to create prompts.
        /// </summary>
        public MenuItemAction()
        {
        }

        /// <summary>
        /// Represents empty menu item action.
        /// This constructor is commonly used to create prompts.
        /// </summary>
        /// <param name="isTerminator">
        /// Indicates whether the top level menu should be 
        /// destroyed after the action invocation.
        /// </param>
        public MenuItemAction(bool isTerminator)
        {
            IsTerminator = isTerminator;
        }

        /// <summary>
        /// Represents menu item action.
        /// </summary>
        /// <param name="action">Action.</param>
        public MenuItemAction(Action<MenuItemActionContext> action)
        {
            Action = action;
        }

        /// <summary>
        /// Represents menu item action.
        /// </summary>
        /// <param name="action">Action.</param>
        public MenuItemAction(Action action)
        {
            Action = mi => action?.Invoke();
        }

        /// <summary>
        /// Gets the menu item action.
        /// </summary>
        public Action<MenuItemActionContext> Action { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether 
        /// the top level menu should be destroyed after the action invocation.
        /// </summary>
        public bool IsTerminator { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether 
        /// the console should be cleared before the action invocation.
        /// </summary>
        public bool ClearBeforeAction { get; set; } = true;

        /// <summary>
        /// Gets or sets a value that indicates whether 
        /// the cursor should be visible during the action invocation.
        /// </summary>
        public bool IsCursorVisible { get; set; } = true;
    }
}
