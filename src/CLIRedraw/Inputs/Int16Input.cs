using System;

namespace CLIRedraw
{
    public class Int16Input : GenericInput<short>
    {
        public Int16Input(string text)
            : base(text, null)
        {
        }

        public Int16Input(string text, string validationMessage)
            : base(text, null, null, validationMessage)
        {
        }

        public Int16Input(string text, Predicate<short> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out short value) => short.TryParse(input, out value) && (Validate == null || Validate(value));
    }
}
