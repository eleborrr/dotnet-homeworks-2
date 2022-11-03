using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1Parameter,
        string operationParameter,
        string val2Parameter)
    {
        var result = 0.0;
        try
        {
            var (val1, operation, val2) =
                Parser.ParseCalcArguments(val1Parameter, operationParameter, val2Parameter);
            result = calculator.Calculate(val1, operation, val2);

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