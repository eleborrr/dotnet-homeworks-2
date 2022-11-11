using System.Diagnostics.CodeAnalysis;
using Hw9;
using Hw9.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddMathCalculator();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Calculator}/{id?}");

app.Run();

namespace Hw9
{
    [ExcludeFromCodeCoverage]
    public partial class Program { }
}

// var shuntingYard = new ShuntingYard();
// var res = shuntingYard.Parse("(5 * 4 + 3 * 2) - 1");
// foreach(var elem in res)
//     Console.Write(elem.ToString());
//
// Console.WriteLine();
//
// var res2= Parser.Parse("(5 * 4 + 3 * 2) - 1");
// var visitor = new Visitor();
// visitor.Visit(res2);
// Console.WriteLine(res2);
