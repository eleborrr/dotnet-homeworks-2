using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly IMemoryCache _memoryCache;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMemoryCache memoryCache ,IMathCalculatorService simpleCalculator)
	{
		_memoryCache = memoryCache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		if (expression is null)
			return await _simpleCalculator.CalculateMathExpressionAsync(expression);
		
		if (_memoryCache.TryGetValue(expression, out double result))
			return new CalculationMathExpressionResultDto(result);
		
		var calculate = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		
		if (calculate.IsSuccess)
		{
			_memoryCache.Set(expression, calculate.Result);
		}
		return calculate;
	}
}