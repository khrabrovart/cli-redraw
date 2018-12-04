using System;

namespace CLIRedraw
{
    public class Int32Input : GenericInput<int>
    {
        public Int32Input(string text)
            : base(text, null)
        {
        }

        public Int32Input(string text, string validationMessage)
            : base(text, null, null, validationMessage)
        {
        }

        public Int32Input(string text, Predicate<int> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out int value) => int.TryParse(input, out value) && (Validate == null || Validate(value));
    }
}
