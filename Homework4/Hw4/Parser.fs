module Hw4.Parser

open System
open Hw4.Calculator
open Microsoft.FSharp.Core


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length <> 3

let parseArg (value: string) =
    match Double.TryParse(value) with
    | true,arg -> arg
    | _ -> raise (ArgumentException "Number is not double")

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> CalculatorOperation.Undefined
    

    
let parseCalcArguments(args : string[]) =
    if isArgLengthSupported args = false
        then raise (ArgumentException "Expected 3 arguments")
    
    let a1 = parseArg args[0]
    let a2 = parseArg args[2]
    let _operation = parseOperation args[1]
    
    if _operation = CalculatorOperation.Undefined
        then raise (InvalidOperationException "Wrong operation")
    
    {arg1 = a1; operation = _operation; arg2 = a2}
   