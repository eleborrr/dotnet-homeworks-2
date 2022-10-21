module Hw6.Parser

open System
open System.Globalization
open Hw6.Calculator
open Hw6.MaybeBuilder


let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
        | 3 -> Ok args
        | _ -> Error "WrongArgLength"
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), string> =
    match operation with
    | CalculatorOperation.Divide -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Multiply -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Minus -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Plus -> Ok (arg1, operation, arg2)
    | _ -> Error $"Could not parse value '{operation}'"

let parseOperation (arg : string) =
    match arg.ToLower() with
    | "plus" -> Ok CalculatorOperation.Plus
    | "minus" -> Ok CalculatorOperation.Minus
    | "multiply" -> Ok CalculatorOperation.Multiply
    | "divide" -> Ok CalculatorOperation.Divide
    | _ -> Error $"Could not parse value '{arg}'"

let parseArg (value: string) =
    match Double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) with
    | true, arg -> Ok arg
    | _ -> Error $"Could not parse value '{value}'"

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), string> =
   maybe{
       let! arg1 = args[0] |> parseArg
       let! arg2 = args[2] |> parseArg
       let! operation = args[1] |> parseOperation
       return (arg1, operation, arg2)
   }

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), string> =
    match operation, arg2 with
        | CalculatorOperation.Divide, 0.0 -> Error "DivideByZero"
        | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe{
        let! argsSupported = args |> isArgLengthSupported
        let! parsedArgs = argsSupported |> parseArgs
        let! operationSupported = parsedArgs |> isOperationSupported
        let! zeroDividing = operationSupported |> isDividingByZero 
        return zeroDividing
    } 