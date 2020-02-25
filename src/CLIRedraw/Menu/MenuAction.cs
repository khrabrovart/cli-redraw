using System;

namespace CLIRedraw
{
    public abstract class MenuAction
    {
        public Action Action { get; protected set; }

        internal bool ShowCursor { get; set; }

        internal bool ClearBeforeAction { get; set; }
    }
}
