using Hw10.Calculator;
using Hw10.Dto;

namespace Hw10.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            var expressionTree = await Task.Run(()=>Parser.Parse(expression));
            var listExpression = new ListExpression(expressionTree);
            var result = await MathExpressionVisitor.VisitAsync(listExpression.Expressions);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}