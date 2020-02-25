using System;

namespace CLIRedraw
{
    /// <summary>
    /// Represents an implicit menu action that is designed for background tasks and DOES NOT clear the console buffer before the invocation.
    /// </summary>
    public class ImplicitMenuAction : MenuAction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitMenuAction"/>.
        /// </summary>
        /// <param name="action">Action.</param>
        public ImplicitMenuAction(Action action)
        {
            Action = action;
        }
    }
}
