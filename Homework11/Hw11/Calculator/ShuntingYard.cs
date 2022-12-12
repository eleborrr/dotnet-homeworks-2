using Hw11.ErrorMessages;

namespace Hw11.Calculator;


public class ShuntingYard
{
    private Stack<string> operations;
    private Queue<QueueData> order;
    private static string[] operators = { "*", "/", "+", "-" };
    private static string[] brackets = { "(", ")" };


    public ShuntingYard()
    {
        operations = new Stack<string>();
        order = new Queue<QueueData>();
    }

    public Queue<QueueData> Parse(string input)
    {
        IsEmptyCheck(input);
        CheckBracketsCount(input);
        CheckInvalidStartEnd(input);
        var transformInput = TransformInput(input);
        CheckValidBracketsContent(transformInput);
        CheckTwoOperationsInRow(transformInput);
        foreach (var symb in transformInput)
        {
            if (IsNumber(symb))
                order.Enqueue(new QueueData(double.Parse(symb)));
            else if (IsOperatorOrBracket(symb))
                CheckOperationsOrder(symb);
            else
            {
                if (char.IsNumber(symb[0]))
                    throw new Exception(MathErrorMessager.NotNumberMessage(symb));
                throw new Exception(MathErrorMessager.UnknownCharacterMessage(symb.ToCharArray()[0]));
            }
        }

        while (operations.Count > 0)
            order.Enqueue(new QueueData(operations.Pop()));
        return order;
    }

    private string[] TransformInput(string input)
    {
        input = input.Replace("(", "( ")
            .Replace(")", " )").Replace("/10", "/ 10");
        return input.Split();
    }

    private void CheckTwoOperationsInRow(string[] input)
    {
        for (int i = 1; i < input.Length; i++)
        {
            if (IsOperator(input[i]) && IsOperator(input[i - 1]))
                throw new Exception(MathErrorMessager.TwoOperationInRowMessage(input[i - 1], input[i]));
        }
    }

    private void CheckValidBracketsContent(string[] input)
    {
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == ")" && IsOperator(input[i - 1]))
                throw new Exception(MathErrorMessager.OperationBeforeParenthesisMessage(input[i - 1]));
            if (input[i-1] == "(" && IsOperator(input[i]))
                throw new Exception(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(input[i]));
        }
    }

    private void CheckInvalidStartEnd(string input)
    {
        if (IsOperator(input[0].ToString()))
            throw new Exception(MathErrorMessager.StartingWithOperation);
        if (IsOperator(input[input.Length - 1].ToString()))
            throw new Exception(MathErrorMessager.EndingWithOperation);
    }

    private void CheckBracketsCount(string input)
    {
        if (input.Count(x => x == '(') != input.Count(x => x == ')'))
            throw new Exception(MathErrorMessager.IncorrectBracketsNumber);
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
    private static bool IsOperatorOrBracket(string inpt) => operators.Contains(inpt) || brackets.Contains(inpt);
}
