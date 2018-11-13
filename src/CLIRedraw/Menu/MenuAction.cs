﻿using System;

namespace CLIRedraw
{
    public class MenuAction
    {
        /// <summary>
        /// Represents menu item action.
        /// </summary>
        public MenuAction()
        {
        }

        /// <summary>
        /// Represents menu item action.
        /// </summary>
        /// <param name="action">Action.</param>
        public MenuAction(Action<MenuActionContext> action)
        {
            Action = action;
        }

        /// <summary>
        /// Represents menu item action.
        /// </summary>
        /// <param name="action">Action.</param>
        public MenuAction(Action action)
        {
            Action = mi => action?.Invoke();
        }

        /// <summary>
        /// Gets the menu item action.
        /// </summary>
        public Action<MenuActionContext> Action { get; set; }

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