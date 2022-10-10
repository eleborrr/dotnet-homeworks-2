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
    args.Length = 3

let parseArg (value: string) =
    match Double.TryParse(value) with
    | true,arg -> arg
    | _ -> raise (ArgumentException $"Expected a number but was {value}")

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> raise (ArgumentException $"Expected \"+\" \"-\" \"\\\" \"*\" but was \"{arg}\"")
    

    
let parseCalcArguments(args : string[]) =
    if isArgLengthSupported args = false
        then raise (ArgumentException "Expected 3 arguments")
    
    let a1 = parseArg args[0]
    let a2 = parseArg args[2]
    let calculation = parseOperation args[1]
    
    {arg1 = a1; operation = calculation; arg2 = a2}
   