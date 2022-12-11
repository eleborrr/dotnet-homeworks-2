using System.Linq.Expressions;

namespace Hw11.Calculator;


public static class Parser
{
    public static Expression Parse(string input)
    {
        var parsedInput = new ShuntingYard().Parse(input);
        var stack = new Stack<Expression>();
        while (parsedInput.Count > 0)
        {
            var elem = parsedInput.Dequeue();
            if (!elem.IsNumber && stack.Count >= 2)
            {
                var right = stack.Pop();
                var left = stack.Pop();
                var operation = GetOperation(elem, left, right);
                stack.Push(operation);
            }
            else
                stack.Push(elem.Number);
        }
        return stack.First();
    }

    public static Expression GetOperation(QueueData elem, Expression num1, Expression num2)
    {
        if (elem.IsNumber)
            throw new ArgumentException();
        return elem.ToString() switch
        {
            "+" => Expression.Add(num1, num2),
            "-" => Expression.Subtract(num1, num2),
            "/" => Expression.Divide(num1, num2),
            "*" => Expression.Multiply(num1, num2),
            _ => throw new ArgumentException()
        };
    }
    
    
}