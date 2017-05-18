using System;
using System.Collections.Generic;
using static DotNetStandardCalculator.DotNetStandardCalculatorConstants;

namespace DotNetStandardCalculator
{
    public static class StandardCalculator
    {
        public static double CalculateFromString(string calculateFrom)
        {
            var split = Utilities.Split(calculateFrom);
            var rpn = ShuntingYard.GetRPNAsArrayFromString(string.Join(" ",split));

            return Calculate(rpn);
        }

        public static double Calculate(string[] rpnSum)
        {
            if (rpnSum.Length == 0)
                throw new Exception("");

            if (rpnSum.Length == 1)
            {
                if (double.TryParse(rpnSum[0], out double parsedValue))
                {
                    return parsedValue;
                }
                else
                {
                    throw new Exception("Welp");
                }
            }

            var stack = new Stack<double>();
            foreach(var token in rpnSum)
            {
                var workingNumber = 0D;
                if (double.TryParse(token, out double parsed))
                    stack.Push(parsed);
                else
                {
                    switch(token)
                    {
                        case OPERATOR_POW:
                            workingNumber = stack.Pop();
                            stack.Push(Math.Pow(stack.Pop(), workingNumber));
                            break;
                        case OPERATOR_MULTIPLY:
                            stack.Push(stack.Pop() * stack.Pop());
                            break;
                        case OPERATOR_ADD:
                            stack.Push(stack.Pop() + stack.Pop());
                            break;
                        case OPERATOR_DIVIDE:
                            workingNumber = stack.Pop();
                            stack.Push(stack.Pop() / workingNumber);
                            break;
                        case OPERATOR_SUBTRACT:
                            workingNumber = stack.Pop();
                            stack.Push(stack.Pop() - workingNumber);
                            break;
                        default:
                            throw new Exception("Unknown operator");
                    }
                }

            }

            return stack.Pop();
        }
    }
}
