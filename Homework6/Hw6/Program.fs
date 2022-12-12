module Hw6.App

open System
open System.Net
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

open Parser
open Calculator

        
let calculateRequest args=
    match args with
            | Ok res -> Ok $"{calculate res}"
            | Error DivideByZero -> Ok "DivideByZero"
            | Error (WrongArgFormat arg) | Error (WrongArgFormatOperation arg) -> Error $"Could not parse value '{arg}'"
            | Error (WrongArgLength arg) -> Error $"Invalid amount of data : expected 3 arguments, but were given {arg}"
            | _ -> Error "Unhandled Error"

let calculatorHandler: HttpHandler =
    fun next ctx ->
        
        let val1 = (ctx.TryGetQueryStringValue "value1").Value
        let operation = (ctx.TryGetQueryStringValue "operation").Value
        let val2 = (ctx.TryGetQueryStringValue "value2").Value
        let args = parseCalcArguments [|val1; operation; val2|]
        // let args = [|val1; operation; val2|]

        
        let result: Result<string, string> = calculateRequest args

        match result with
            | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
            | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use /calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp
            
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0