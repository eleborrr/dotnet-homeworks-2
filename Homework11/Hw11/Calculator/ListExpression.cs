using System.Linq.Expressions;

namespace Hw11.Calculator;

public class ListExpression
{
    public List<Expression> Expressions { get; }

    public ListExpression(Expression expression)
    {
        Expressions = new List<Expression>();
        
        Visit(expression);
    }

    private void Visit(Expression expression)
    {
        if(expression is BinaryExpression binaryExpression)
            Visit(binaryExpression);
        else if(expression is ConstantExpression constantExpression)
            Visit(constantExpression);
    }

    private void Visit(BinaryExpression? expression)
    {
        Expressions.Add(expression);
        Visit(expression.Left);
        Visit(expression.Right);
    }

    private void Visit(ConstantExpression? expression)
    {
        Expressions.Add(expression);
    }
}