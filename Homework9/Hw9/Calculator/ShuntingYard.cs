using System.Linq.Expressions;
using Hw9.ErrorMessages;
using Microsoft.AspNetCore.Identity;

namespace Hw9;


public class ShuntingYard
{
    private Stack<string> operations;
    private Queue<QueueData> order;
    private static string[] operators = { "(", ")", "*", "/", "+", "-" };

    public ShuntingYard()
    {
        operations = new Stack<string>();
        order = new Queue<QueueData>();
    }

    public Queue<QueueData> Parse(string input)
    {
        IsEmptyCheck(input);
        foreach (var symb in TransformInput(input))
        {
            if (IsNumber(symb))
                order.Enqueue(new QueueData(double.Parse(symb)));
            else if (IsOperator(symb))
                CheckOperationsOrder(symb);
            else
                throw new Exception(MathErrorMessager.UnknownCharacterMessage('a'));
        }

        while (operations.Count > 0)
            order.Enqueue(new QueueData(operations.Pop()));
        return order;
    }

    private string[] TransformInput(string input)
    {
        input = input.Replace("(", "( ")
            .Replace(")", " )");
        return input.Split();
    }

    private void CheckOperationsOrder(string inpt)
    {
        if (inpt is "+" or "-")
        {
            if (operations.Count > 0)
            {
                string stackOperator = "";
                do
                {
                    stackOperator = operations.Pop();
                    if (stackOperator is "*" or "/")
                        order.Enqueue(new QueueData(stackOperator));
                } while (stackOperator is "*" or "/" && operations.Count > 0);

                if(stackOperator == "(")
                    operations.Push(stackOperator);

            }

            operations.Push(inpt);
        }
        else if (inpt is ")")
        {
            var operation = operations.Pop();
            while (operations.Count > 0 && operation != "(")
            {
                order.Enqueue(new QueueData(operation));
                operation = operations.Pop();
            }
        }
        else
            operations.Push(inpt);

    }

    private void IsEmptyCheck(string input)
    {
        if (input is "" or null)
            throw new Exception(MathErrorMessager.EmptyString);
    }
    
    
    private static bool IsNumber(string input)
    {
        double number;
        return Double.TryParse(input, out number);
    }

    private static bool IsOperator(string inpt) => operators.Contains(inpt);
}
