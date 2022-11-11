using System.Linq.Expressions;
using System.Reflection.Metadata;
using Hw9.ErrorMessages;

namespace Hw9;

public class Visitor: ExpressionVisitor
{
    public double Start(Expression node)
    {
        Thread.Sleep(1000);
        Visit(node);
        if (node.NodeType == ExpressionType.Constant)
            return (double)((ConstantExpression)node).Value;
        var nodeBinary = (BinaryExpression)node;
        var val1 = Task.Run(() => Visit(nodeBinary.Left));
        var val2 = Task.Run(() => Visit(nodeBinary.Right));
        // var val1 = Task.Run(() => Expression.Lambda<Func<double>>(nodeBinary.Left).Compile().Invoke());
        // var val2 = Task.Run(() => Expression.Lambda<Func<double>>(nodeBinary.Right).Compile().Invoke());
        Task.WhenAll(val1, val2);
        //return GetExpression(node, val1.Result, val2.Result);
        return 2;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        // var val1 = Task.Run(() => Expression.Lambda<Func<double>>(node.Left).Compile().Invoke());
        // var val2 = Task.Run(() => Expression.Lambda<Func<double>>(node.Right).Compile().Invoke());
        Visit(node.Right);
        // Task.WhenAll(val1, val2);
        return node;
        // return GetExpression(node, val1.Result, val2.Result);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        Visit(node);
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        return node;
    }
    
    private static Expression GetExpression(Expression expression, double left, double right)
    {
        Thread.Sleep(1000);
        return expression.NodeType switch
        {
            ExpressionType.Add => Expression.Constant(left + right),
            ExpressionType.Subtract => Expression.Constant(left - right),
            ExpressionType.Multiply => Expression.Constant(left * right),
            ExpressionType.Divide =>
                right != 0.0 
                    ? Expression.Constant(left / right) 
                    : throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            ExpressionType.Constant => ((ConstantExpression)expression)
        };
    }

    private static double Calculate(Expression expression, double left, double right)
    {
        return expression.NodeType switch
        {
            ExpressionType.Add => left + right,
            ExpressionType.Subtract => left + right,
            ExpressionType.Multiply => left * right,
            ExpressionType.Divide =>
                right != 0.0 
                    ? left / right 
                    : throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            ExpressionType.Constant => (double)((ConstantExpression)expression).Value
        };
    }
    
}