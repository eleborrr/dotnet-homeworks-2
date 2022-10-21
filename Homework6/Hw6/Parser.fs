module Hw6.Parser

open System
open System.Globalization
open Hw6.Calculator
open Hw6.MaybeBuilder


type Message =
    | SuccessfulExecution of string
    | WrongArgLength of string
    | WrongArgFormat of string
    | WrongArgFormatOperation of string
    | DivideByZero

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
        | 3 -> Ok args
        | _ -> Error (WrongArgLength($"{args.Length}"))
        // | _ -> Error "WrongArgLength"

    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | "plus" | Calculator.plus -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | "minus" | Calculator.minus -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "multiply" | Calculator.multiply -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "divide" | Calculator.divide -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | _ -> Error (WrongArgFormatOperation(operation))
    // | _ -> Error $"Could not parse value '{operation}'"

let parseOperation (arg : string) =
    match arg.ToLower() with
    | "plus" -> Ok CalculatorOperation.Plus
    | "minus" -> Ok CalculatorOperation.Minus
    | "multiply" -> Ok CalculatorOperation.Multiply
    | "divide" -> Ok CalculatorOperation.Divide
    | _ -> Error (WrongArgFormatOperation(arg))
    // | _ -> Error $"Could not parse value '{arg}'"

let parseArg (value: string) =
    match Double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) with
    | true, arg -> Ok arg
    | _ -> Error (WrongArgFormat(value))
    // | _ -> Error $"Could not parse value '{value}'"

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
   maybe{
       let! arg1 = args[0] |> parseArg
       let! arg2 = args[2] |> parseArg
       let! operation = args[1] |> parseOperation
       return (arg1, operation, arg2)
   }

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation, arg2 with
        // | CalculatorOperation.Divide, 0.0 -> Error "DivideByZero"
        | CalculatorOperation.Divide, 0.0 -> Error Message.DivideByZero
        | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe{
        let! argsSupported = args |> isArgLengthSupported
        let! parsedArgs = argsSupported |> parseArgs
        let! zeroDividing = parsedArgs |> isDividingByZero 
        return zeroDividing
    } 