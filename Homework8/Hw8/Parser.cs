using System.Globalization;

namespace Hw8;
using Hw8.Calculator;

public static class Parser
{
    public static (double, Operation, double) ParseCalcArguments(
        string val1,
        string operationParam,
        string val2)
    {
        var num1 = 0.0;
        var num2 = 0.0;
        try
        {
            num1 = double.Parse(val1, CultureInfo.InvariantCulture);
            num2 = double.Parse(val2, CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            throw new ArgumentException(Messages.InvalidNumberMessage);
        }
        var operation = ParseOperation(operationParam.ToLower());
        if (operation == Operation.Invalid)
            throw new InvalidOperationException(Messages.InvalidOperationMessage);
        return (num1, operation, num2);
    }
    
    private static Operation ParseOperation(string arg)
    {
        return (arg) switch
        {
            "plus" => Operation.Plus,
            "minus" => Operation.Minus,
            "multiply" => Operation.Multiply,
            "divide" => Operation.Divide,
            _ => Operation.Invalid
        };
    }
}