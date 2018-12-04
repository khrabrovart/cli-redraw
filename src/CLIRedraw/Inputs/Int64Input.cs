using System;

namespace CLIRedraw
{
    public class Int64Input : GenericInput<long>
    {
        public Int64Input(string text)
            : base(text, null)
        {
        }

        public Int64Input(string text, string validationMessage)
            : base(text, null, null, validationMessage)
        {
        }

        public Int64Input(string text, Predicate<long> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out long value) => long.TryParse(input, out value) && (Validate == null || Validate(value));
    }
}
