using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        var result = 0.0;
        try
        {
            var parsedData = Parser.ParseCalcArguments(val1, operation, val2);

            result = calculator.Calculate(parsedData.Item1, parsedData.Item2, parsedData.Item3);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}