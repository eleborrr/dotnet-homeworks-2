module Hw4.Calculator

open System

type CalculatorOperation =
     | Plus = 0
     | Minus = 1
     | Multiply = 2
     | Divide = 3
     | Undefined = 4
     
let Plus x y =
    x + y
    
let Minus x y =
    x - y
    
let Divide x y =
    x / y
    
let Multiply x y =
    x * y
     
let calculate (value1 : float) (operation : CalculatorOperation) (value2 : float) =
    match operation with 
    | CalculatorOperation.Plus -> Plus value1 value2
    | CalculatorOperation.Minus -> Minus value1 value2
    | CalculatorOperation.Multiply -> Multiply value1 value2
    | CalculatorOperation.Divide -> Divide value1 value2
    | CalculatorOperation.Undefined | _ -> raise (ArgumentOutOfRangeException "Unknown operation")
    
