using System;

namespace CLIRedraw
{
    public class GenericInput<T>
    {
        private const string DefaultValidationMessage = "Invalid input.";

        public GenericInput(string text, Func<string, T> format)
            : this(text, format, null, null)
        {
        }

        public GenericInput(string text, Func<string, T> format, Predicate<T> validate, string validationMessage)
        {
            Text = text;
            Format = format;
            Validate = validate;
            ValidationMessage = validationMessage;
        }

        public string Text { get; set; }

        public string ValidationMessage { get; set; }

        public Func<string, T> Format { get; set; }

        public Predicate<T> Validate { get; set; }

        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

        public bool ClearBeforeInput { get; set; } = true;

        public static GenericInput<T> Create(string text, Func<string, T> format)
        {
            return new GenericInput<T>(text, format);
        }

        public static GenericInput<T> Create(string text, Func<string, T> format, Predicate<T> validate, string validationMessage)
        {
            return new GenericInput<T>(text, format, validate, validationMessage);
        }

        public static T Show(string text, Func<string, T> format)
        {
            return Create(text, format).Show();
        }

        public static T Show(string text, Func<string, T> format, Predicate<T> validate, string validationMessage)
        {
            return Create(text, format, validate, validationMessage).Show();
        }

        public T Show()
        {
            if (ClearBeforeInput)
            {
                Console.Clear();
            }

            ColorConsole.Write($"{Text}: ", BackgroundColor, ForegroundColor);

            var input = Console.ReadLine();

            if (!IsValid(input, out var formattedInput))
            {
                if (ClearBeforeInput)
                {
                    Console.Clear();
                }

                ColorConsole.WriteLine(
                    string.IsNullOrWhiteSpace(ValidationMessage) ? DefaultValidationMessage : ValidationMessage, 
                    foregroundColor: ConsoleColor.Red);

                if (ClearBeforeInput)
                {
                    Console.ReadKey();
                }

                return Show();
            }

            return formattedInput;
        }

        protected virtual bool IsValid(string input, out T value)
        {
            if (Format == null)
            {
                value = default;
                return true;
            }

            value = Format(input);
            return Validate == null || Validate(value);
        }
    }
}
