using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetStandardCalculator
{
    public static class ShuntingYard
    {
        
        private static Regex isOpenParentheseseRegex = new Regex("\\(");
        private static Regex isCloseParentheseseRegex = new Regex("\\)");
        private static Regex isOperatorRegex = new Regex("[*/%+\\-]");

        public static string[] GetRPNAsArrayFromString(string infixNotation)
        {
            return Parse(infixNotation.Split(' '));
        }

        public static string[] GetRPNAsArrayFromString(string[] infixArray)
        {
            return Parse(infixArray);
        }

        // http://en.wikipedia.org/wiki/Shunting-yard_algorithm
        private static string[] Parse(string[] tokens)
        {
            Stack<string> valuesStack = new Stack<string>();
            Stack<char> symbolStack = new Stack<char>();

            foreach (var token in tokens)
            {
                if (Utilities.IsNumber(token))
                {
                    valuesStack.Push(token);
                }
                else if (isOpenParentheseseRegex.IsMatch(token))
                {
                    symbolStack.Push(token[0]);
                }
                else if (isCloseParentheseseRegex.IsMatch(token))
                {
                    while (symbolStack.Any())
                    {
                        if (symbolStack.Peek().Equals('('))
                        {
                            symbolStack.Pop();
                            break;
                        }

                        valuesStack.Push(symbolStack.Pop().ToString());
                    }
                }
                else if (isOperatorRegex.IsMatch(token))
                {
                    while (symbolStack.Any())
                    {
                        if (!symbolStack.Peek().Equals("("))
                            if (OperatorPrecedence(symbolStack.Peek()) >= OperatorPrecedence(token[0]))
                            {
                                valuesStack.Push(symbolStack.Pop().ToString());
                            }
                            else
                                break;
                    }
                    symbolStack.Push(token[0]);
                }
            }

            while (symbolStack.Any())
                valuesStack.Push(symbolStack.Pop().ToString());
            return valuesStack.Reverse().ToArray();
        }

        private static int OperatorPrecedence(char op)
        {
            switch (op)
            {
                case '*':
                case '/':
                case '%':
                    return 5;
                case '-':
                case '+':
                    return 4;
            }
            return 0;
        }
    }
}
