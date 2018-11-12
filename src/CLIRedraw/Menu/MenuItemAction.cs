using System;

namespace CLIRedraw
{
    public class MenuItemAction
    {
        public MenuItemAction(Action<MenuItem> action)
        {
            Action = action;
        }

        public MenuItemAction(Action action)
        {
            Action = mi => action?.Invoke();
        }

        public Action<MenuItem> Action { get; }

        public bool IsTerminator { get; set; }

        public bool ClearBeforeAction { get; set; } = true;

        public bool ShowCursor { get; set; } = true;
    }
}
