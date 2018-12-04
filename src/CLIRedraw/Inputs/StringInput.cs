using System;

namespace CLIRedraw
{
    public class StringInput : GenericInput<string>
    {
        public StringInput(string text)
            : base(text, null)
        {
        }

        public StringInput(string text, Predicate<string> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out string value)
        {
            if (Validate == null || Validate(input))
            {
                value = input;
                return true;
            }

            value = null;
            return false;
        }
    }
}
