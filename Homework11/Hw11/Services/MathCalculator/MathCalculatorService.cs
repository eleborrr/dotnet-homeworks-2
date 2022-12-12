using Hw11.Dto;
using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator;
using Hw11.Calculator;

public class MathCalculatorService : IMathCalculatorService
{
    private readonly IExceptionHandler _handler;

    public MathCalculatorService(IExceptionHandler handler)
    {
        _handler = handler;
    }
    
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            var expressionTree = await Task.Run(()=>Parser.Parse(expression));
            var listExpression = new ListExpression(expressionTree);
            var result = await MathExpressionVisitor.VisitAsync(listExpression.Expressions);
            return result;
        }
        catch (Exception ex)
        {
            _handler.HandleException(ex);
            throw;
        }
    }
}