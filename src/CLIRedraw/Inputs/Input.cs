using System;
using System.Text.RegularExpressions;

namespace CLIRedraw
{
    public static class Input
    {
        public static string String(string text)
        {
            return new StringInput(text).Show();
        }

        public static string String(string text, Predicate<string> validate, string validationMessage)
        {
            return new StringInput(text, validate, validationMessage).Show();
        }

        public static string Regex(string text, Regex regex)
        {
            return new RegexInput(text, regex).Show();
        }

        public static string Regex(string text, Regex regex, string validationMessage)
        {
            return new RegexInput(text, regex, validationMessage).Show();
        }

        public static string Regex(string text, string pattern)
        {
            return new RegexInput(text, pattern).Show();
        }

        public static string Regex(string text, string pattern, string validationMessage)
        {
            return new RegexInput(text, pattern, validationMessage).Show();
        }

        public static byte Byte(string text)
        {
            return new ByteInput(text).Show();
        }

        public static byte Byte(string text, string validationMessage)
        {
            return new ByteInput(text, validationMessage).Show();
        }

        public static byte Byte(string text, Predicate<byte> validate, string validationMessage)
        {
            return new ByteInput(text, validate, validationMessage).Show();
        }

        public static short Int16(string text)
        {
            return new Int16Input(text).Show();
        }

        public static short Int16(string text, string validationMessage)
        {
            return new Int16Input(text, validationMessage).Show();
        }

        public static short Int16(string text, Predicate<short> validate, string validationMessage)
        {
            return new Int16Input(text, validate, validationMessage).Show();
        }

        public static int Int32(string text)
        {
            return new Int32Input(text).Show();
        }

        public static int Int32(string text, string validationMessage)
        {
            return new Int32Input(text, validationMessage).Show();
        }

        public static int Int32(string text, Predicate<int> validate, string validationMessage)
        {
            return new Int32Input(text, validate, validationMessage).Show();
        }

        public static long Int64(string text)
        {
            return new Int64Input(text).Show();
        }

        public static long Int64(string text, string validationMessage)
        {
            return new Int64Input(text, validationMessage).Show();
        }

        public static long Int64(string text, Predicate<long> validate, string validationMessage)
        {
            return new Int64Input(text, validate, validationMessage).Show();
        }

        public static float Float(string text)
        {
            return new FloatInput(text).Show();
        }

        public static float Float(string text, string validationMessage)
        {
            return new FloatInput(text, validationMessage).Show();
        }

        public static float Float(string text, Predicate<float> validate, string validationMessage)
        {
            return new FloatInput(text, validate, validationMessage).Show();
        }

        public static double Double(string text)
        {
            return new DoubleInput(text).Show();
        }

        public static double Double(string text, string validationMessage)
        {
            return new DoubleInput(text, validationMessage).Show();
        }

        public static double Double(string text, Predicate<double> validate, string validationMessage)
        {
            return new DoubleInput(text, validate, validationMessage).Show();
        }
    }
}
