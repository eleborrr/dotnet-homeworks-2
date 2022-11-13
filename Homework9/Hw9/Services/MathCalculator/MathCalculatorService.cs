using System.Linq.Expressions;
using Hw9.Dto;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    private string[] _operations = new string[] { "*", "/", "+", "-" };
    
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var visitor = new Visitor();
        var expressionTree = await Task.Run(()=>Parser.Parse(expression));
        try
        {
            //var taskExpression = await Task.Run(() => visitor.Visit(expressionTree));
            var listExpression = new ListExpression(expressionTree);
            var result = await MathExpressionVisitor.VisitAsync(listExpression.Expressions);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (DivideByZeroException ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}