open System
open Hw5.Calculator
open Hw5.Parser
open Microsoft.FSharp.Core

let calculateFunc (arg1, operation, arg2) =
    calculate arg1 operation arg2

[<EntryPoint>]
let Main args =
    match parseArgs args with
    | Ok res -> printfn $"{calculateFunc res}"
    | Error error -> printfn $"{error}"
    0