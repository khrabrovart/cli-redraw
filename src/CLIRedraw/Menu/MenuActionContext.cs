namespace CLIRedraw
{
    public sealed class MenuActionContext
    {
        public MenuActionContext(Menu menu, MenuItem menuItem, MenuAction menuAction)
        {
            Menu = menu;
            MenuItem = menuItem;
            MenuAction = menuAction;
        }

        /// <summary>
        /// Gets the menu.
        /// </summary>
        public Menu Menu { get; private set; }

        /// <summary>
        /// Gets the menu item that invoked the action.
        /// </summary>
        public MenuItem MenuItem { get; private set; }

        /// <summary>
        /// Gets the action.
        /// </summary>
        public MenuAction MenuAction { get; private set; }
    }
}
