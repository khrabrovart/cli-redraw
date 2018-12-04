using System;

namespace CLIRedraw
{
    public class DoubleInput : GenericInput<double>
    {
        public DoubleInput(string text)
            : base(text, null)
        {
        }

        public DoubleInput(string text, string validationMessage)
            : base(text, null, null, validationMessage)
        {
        }

        public DoubleInput(string text, Predicate<double> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out double value) => double.TryParse(input, out value) && (Validate == null || Validate(value));
    }
}
