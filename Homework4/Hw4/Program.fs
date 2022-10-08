open System
open Hw4.Calculator
open Hw4.Parser

let work = true
let input = [|" "|]
while input[0] <> "exit" do
    let input = Console.ReadLine().Split()
    let parseArgs = parseCalcArguments input
    let res = calculate parseArgs.arg1 parseArgs.operation parseArgs.arg2
    printfn $"{res}"
  