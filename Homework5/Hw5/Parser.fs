module Hw5.Parser

open System
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
        | 3 -> Ok args
        | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | CalculatorOperation.Divide -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Multiply -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Minus -> Ok (arg1, operation, arg2)
    | CalculatorOperation.Plus -> Ok (arg1, operation, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseOperation (arg : string) =
    match arg with
    | "+" -> Ok CalculatorOperation.Plus
    | "-" -> Ok CalculatorOperation.Minus
    | "*" -> Ok CalculatorOperation.Multiply
    | "/" -> Ok CalculatorOperation.Divide
    | _ -> Error Message.WrongArgFormatOperation

let parseArg (value: string) =
    match Double.TryParse(value) with
    | true, arg -> Ok arg
    | _ -> Error Message.WrongArgFormat

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
        | CalculatorOperation.Divide, 0.0 -> Error Message.DivideByZero
        | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe{
        let! argsSupported = args |> isArgLengthSupported
        let! parsedArgs = argsSupported |> parseArgs
        let! operationSupported = parsedArgs |> isOperationSupported
        let! zeroDividing = operationSupported |> isDividingByZero 
        return zeroDividing
    } 