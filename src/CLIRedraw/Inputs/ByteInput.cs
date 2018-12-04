using System;

namespace CLIRedraw
{
    public class ByteInput : GenericInput<byte>
    {
        public ByteInput(string text)
            : base(text, null)
        {
        }

        public ByteInput(string text, string validationMessage)
            : base(text, null, null, validationMessage)
        {
        }

        public ByteInput(string text, Predicate<byte> validate, string validationMessage)
            : base(text, null, validate, validationMessage)
        {
        }

        protected override bool IsValid(string input, out byte value) => byte.TryParse(input, out value) && (Validate == null || Validate(value));
    }
}
