using System;

namespace CLIRedraw
{
    public class FloatInput : GenericInput<float>
    {
        public FloatInput(string text)
            : base(text, null)
        {
        }

        public FloatInput(string text, string validationMessage)
            : base(text, null, null, validationMessage)
        {
        }

        public FloatInput(string text, Predicate<float> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out float value) => float.TryParse(input, out value) && (Validate == null || Validate(value));
    }
}
