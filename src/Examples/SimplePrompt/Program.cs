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
            var prompt = new Menu();

            var yesMenuItem = prompt.Add("Yes", context => 
            {
                context.Menu.Close();
                parentContext.Menu.Close();
            });

            var noMenuItem = prompt.Add("No", context => context.Menu.Close());

            prompt.Show();
        }
    }
}
