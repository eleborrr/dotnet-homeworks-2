using System.Linq.Expressions;
using Hw9.Dto;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    private string[] _operations = new string[] { "*", "/", "+", "-" };
    
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var visitor = new Visitor();
        var expr = await Task.Run(()=>Parser.Parse(expression));
        try
        {
            var result = visitor.Visit(expr);
            var r = Expression.Lambda<Func<double>>(result).Compile().Invoke();
            return new CalculationMathExpressionResultDto(r);
        }
        catch (DivideByZeroException ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}