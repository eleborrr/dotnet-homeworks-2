open System
open Hw4.Calculator
open Hw4.Parser

[<EntryPoint>]
let Main args =
    let parsedArgs = parseCalcArguments args
    let res = calculate parsedArgs.arg1 parsedArgs.operation parsedArgs.arg2
    printfn $"{res}"
    0