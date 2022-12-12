using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var props = helper.ViewData.ModelMetadata.ModelType.GetProperties();
        
        var builder = new HtmlContentBuilder();

        foreach (var prop in props)
        {
            builder.AppendHtmlLine("<div>");
            var display = GetDisplay(prop);
            var attr = GetAttr(prop, model);
            builder.AppendHtmlLine($"<label for=\"{prop.Name}\">{display}</label><br> " +
                                   $"<span asp-validation-for\"{prop.Name}\">{attr}</span>");
            builder.AppendHtmlLine(GetInput(prop, model));

            builder.AppendHtmlLine("</div>");
        }
        return builder;
    }

    private static string GetAttr(PropertyInfo prop, object? model)
    {
        var v = prop.GetCustomAttributes<ValidationAttribute>();
        if (model != null)
        {
            var value = prop.GetValue(model);
            foreach (var attribute in v)
            {
                if (!attribute.IsValid(value))
                    return attribute.ErrorMessage!;
            }
        }
        return "";
    }
    
    private static string GetInput(PropertyInfo prop, object? model)
    {
        var builder = new StringBuilder();
        var type = prop.PropertyType;
        if (!type.IsEnum)
        {
            var contentType = type == typeof(string) ? "text" : "number";
            return $"<input id=\"{prop.Name}\" name=\"{prop.Name}\" type=\"{contentType}\"></input><br>";
        }
        else
        {
            var modelValue = model == null ? "" : prop.GetValue(model);
            var enumValues = type.GetEnumValues();
            builder.Append($"<select id=\"{prop.Name}\" name=\"{prop.Name}\" value=\"{modelValue}\">");
            foreach (var enumValue in enumValues)
            {
                builder.Append($"<br><option>{enumValue}</option>");
            }

            builder.Append("</select");
            return builder.ToString();
        }
    }
    
    private static string GetDisplay(PropertyInfo prop)
    {
        var display = prop.GetCustomAttribute<DisplayAttribute>();
        if (display != null)
            return display.Name!;
        return SplitCamelCase(prop.Name);
    }
    
    private static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }
} 