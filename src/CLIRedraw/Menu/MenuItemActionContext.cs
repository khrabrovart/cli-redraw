namespace CLIRedraw
{
    public class MenuItemActionContext
    {
        internal MenuItemActionContext(Menu menu, MenuItem menuItem, MenuItemAction menuItemAction)
        {
            Menu = menu;
            Item = menuItem;
            Action = menuItemAction;
        }

        /// <summary>
        /// Gets the menu.
        /// </summary>
        public Menu Menu { get; private set; }

        /// <summary>
        /// Gets the menu item that invoked the action.
        /// </summary>
        public MenuItem Item { get; private set; }

        /// <summary>
        /// Gets the action.
        /// </summary>
        public MenuItemAction Action { get; private set; }
    }
}
