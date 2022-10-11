module Hw4.Calculator

open System

type CalculatorOperation =
     | Plus = 0
     | Minus = 1
     | Multiply = 2
     | Divide = 3
     | Undefined = 4
     
let plus x y =
    x + y
    
let minus x y =
    x - y
    
let divide x y =
    x / y
    
let multiply x y =
    x * y
     
let calculate (value1 : float) (operation : CalculatorOperation) (value2 : float) =
    match operation with 
    | CalculatorOperation.Plus -> plus value1 value2
    | CalculatorOperation.Minus -> minus value1 value2
    | CalculatorOperation.Multiply -> multiply value1 value2
    | CalculatorOperation.Divide -> divide value1 value2
    | _ -> raise (ArgumentOutOfRangeException $"Expected {CalculatorOperation} but was \"{operation}\"")
    
