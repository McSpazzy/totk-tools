using System.Text.RegularExpressions;

namespace ToolLib
{
    public partial class ToolFunctions
    {
        [GeneratedRegex("(?<!^)(?=[A-Z])", RegexOptions.Compiled)]
        private static partial Regex SplitCamelRegex();

        [GeneratedRegex("^[a-zA-Z0-9.\\s,]*$", RegexOptions.Compiled)]
        private static partial Regex IsAlphaNumeric();


        public static string[] SplitCamelCase(string input)
        {
            return SplitCamelRegex().Split(input);
        }

        public static bool IsAlphaNumeric(string input)
        {
            return IsAlphaNumeric().IsMatch(input);
        }

        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            {
                return input;
            }
            return char.ToUpper(input[0]) + input[1..];
        }
    }
}
