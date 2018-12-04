using System.Text.RegularExpressions;

namespace CLIRedraw
{
    public class RegexInput : GenericInput<string>
    {
        private readonly Regex _regex;

        public RegexInput(string text, Regex regex)
            : base(text, null)
        {
            _regex = regex;
        }

        public RegexInput(string text, Regex regex, string validationMessage)
            : base(text, null, null, validationMessage)
        {
            _regex = regex;
        }

        public RegexInput(string text, string pattern)
            : base(text, null)
        {
            _regex = new Regex(pattern);
        }

        public RegexInput(string text, string pattern, string validationMessage)
            : base(text, null, null, validationMessage)
        {
            _regex = new Regex(pattern);
        }

        protected override bool IsValid(string input, out string value)
        {
            if (_regex.IsMatch(input))
            {
                value = input;
                return true;
            }

            value = null;
            return false;
        }
    }
}
