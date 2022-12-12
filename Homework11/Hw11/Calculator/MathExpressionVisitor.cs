using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw11.Calculator;

public static class MathExpressionVisitor
{
    private static Dictionary<Expression, Lazy<Task<double>>> nodes = new Dictionary<Expression, Lazy<Task<double>>>();

    public static async Task<double> VisitAsync(List<Expression> expressionList)
    {

        for(int i = 0; i < expressionList.Count; i++)
        {
            var index = i;
            nodes[expressionList[i]] =
                new Lazy<Task<double>>(async () => await HandleExpression((dynamic)expressionList[index]));
        }

        return await nodes[expressionList[0]].Value;
    }
    
    private static async Task<double> HandleExpression(BinaryExpression binaryExpression)
    {
        await Task.WhenAll(nodes[binaryExpression.Left].Value, nodes[binaryExpression.Right].Value);
        await Task.Yield();
        await Task.Delay(1000);
        return GetExpressionResult(binaryExpression, await nodes[binaryExpression.Left].Value,
            await nodes[binaryExpression.Right].Value);
    } 
    

    private static async Task<double> HandleExpression(ConstantExpression constantExpression)
    {
        return (double)constantExpression.Value;
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