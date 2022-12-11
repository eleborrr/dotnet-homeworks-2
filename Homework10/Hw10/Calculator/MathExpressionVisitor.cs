using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw10.Calculator;

public static class MathExpressionVisitor
{
    public static async Task<double> VisitAsync(List<Expression> expressionList)
    {
        var nodes = new Dictionary<Expression, Lazy<Task<double>>>();

        for(int i = 0; i < expressionList.Count; i++)
        {
            var index = i;
            nodes[expressionList[i]] = new Lazy<Task<double>>(async () =>
            {
                if (expressionList[index] is BinaryExpression binaryExpression)
                {
                    await Task.WhenAll(nodes[binaryExpression.Left].Value, nodes[binaryExpression.Right].Value);
                    await Task.Yield();
                    await Task.Delay(1000);
                    return GetExpressionResult(binaryExpression, nodes[binaryExpression.Left].Value.Result,
                        nodes[binaryExpression.Right].Value.Result);
                }
                return (double)(expressionList[index] as ConstantExpression).Value;
            });
        }

        return await nodes[expressionList[0]].Value;
    }

    [ExcludeFromCodeCoverage]
    private static double GetExpressionResult(BinaryExpression node, double val1, double val2) => node.NodeType switch
    {
        
        ExpressionType.Add => val1 + val2,
        ExpressionType.Subtract => val1 - val2,
        ExpressionType.Multiply => val1 * val2,
        ExpressionType.Divide => val2 == 0
            ? throw new Exception(ErrorMessages.MathErrorMessager.DivisionByZero)
            : val1 / val2,
        _ => throw new Exception(ErrorMessages.MathErrorMessager.UnknownCharacter)
    };
}