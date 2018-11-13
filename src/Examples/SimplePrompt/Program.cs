using CLIRedraw;

namespace SimplePrompt
{
    class Program
    {
        public static void Main(string[] args)
        {
            var menu = new Menu("Simple Prompt");
            menu.Add("Exit", ExitPrompt);
            menu.Show();
        }

        private static void ExitPrompt(MenuActionContext parentContext)
        {
            var yesMenuAction = new MenuAction(() => parentContext.MenuAction.IsTerminator = true)
            {
                IsTerminator = true
            };

            var noMenuAction = new MenuAction(() => parentContext.MenuAction.IsTerminator = false)
            {
                IsTerminator = true
            };

            var prompt = new Menu();

            prompt.Add("Yes", yesMenuAction);
            prompt.Add("No", noMenuAction);

            prompt.Show();
        }
    }
}
