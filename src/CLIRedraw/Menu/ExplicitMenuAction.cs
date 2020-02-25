using System;

namespace CLIRedraw
{
    /// <summary>
    /// Represents an explicit menu action that clears the console buffer before the invocation.
    /// </summary>
	public class ExplicitMenuAction : MenuAction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitMenuAction"/>.
        /// </summary>
        /// <param name="action">Action.</param>
        public ExplicitMenuAction(Action action)
        {
            Action = action;
            ShowCursor = true;
            ClearBeforeAction = true;
        }
    }
}
