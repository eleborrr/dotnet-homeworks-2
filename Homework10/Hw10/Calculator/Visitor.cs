using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw10.Calculator;

[ExcludeFromCodeCoverage]
public class Visitor : ExpressionVisitor
{
    private static readonly Dictionary<Expression, Lazy<Task<double>>> Nodes = new();

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Nodes[node] = new Lazy<Task<double>>(async () =>
        {
            await Task.WhenAll(Nodes[node.Left].Value, Nodes[node.Right].Value);

            return GetExpressionResult(node, Nodes[node.Left].Value.Result, Nodes[node.Right].Value.Result);
        });
        return base.VisitBinary(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        Nodes[node] = new Lazy<Task<double>>(async () => (double)node.Value);
        return base.VisitConstant(node);
    }

    private double GetExpressionResult(BinaryExpression node, double val1, double val2) => node.NodeType switch
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