using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetStandardCalculator
{
    public static class Utilities
    {
        private static Regex isNumberRegex = new Regex("^([0-9]+)?([.][0-9]+)?$");

        private static string[] operatorsInOrder = new[] { "^", "*", "/", "+", "-", "(", ")" };

        public static string[] Split(string stringToSplit)
        {
            var operatorsRegex = string.Join("|", operatorsInOrder.Select(d => Regex.Escape(d)).ToArray());

            var pattern = $"({operatorsRegex})";
            var split = Regex.Split(stringToSplit, pattern);
            var splitAndTrimmed = split.Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            var splitValidated = Array.Empty<string>();
            return TryValidate(splitAndTrimmed, out splitValidated) ? splitValidated : null;
        }

        private static bool TryValidate(string[] unvalidatedCollection, out string[] validated)
        {
            //TODO: fix any that are fixable?
            validated = unvalidatedCollection;

            return true;
        }

        internal static bool IsNumber(string str) => isNumberRegex.Match(str).Success;
    }
}
