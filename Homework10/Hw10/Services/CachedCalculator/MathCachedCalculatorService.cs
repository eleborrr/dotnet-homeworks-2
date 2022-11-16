using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var result = _dbContext.SolvingExpressions.FirstOrDefault(exp => exp.Expression == expression);
		if (result != null) return new CalculationMathExpressionResultDto(result.Result);
		
		var expr = new SolvingExpression();
		expr.Expression = expression;
		var calculated = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		expr.Result = calculated.Result;

		if (calculated.IsSuccess)
		{
			_dbContext.SolvingExpressions.Add(expr);
			await _dbContext.SaveChangesAsync();
		}

		return calculated;

	}
}