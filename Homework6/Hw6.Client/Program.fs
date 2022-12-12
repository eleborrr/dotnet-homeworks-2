open System
open System.Net.Http

let fetchAsync =
    async{
        use client = new HttpClient()
        while true do
            try
                let args = Console.ReadLine().Split()
                if args.Length <> 3 then
                    raise (ArgumentException("wrong arguments length"))
                let uri = Uri $"https://localhost:5001/calculate?value1={args[0]}&operation={args[1]}&value2={args[2]}"                
                let! response = client.GetAsync(uri) |> Async.AwaitTask
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                printfn $"{content}"
            with
                | ex ->
                    printfn "%s" (ex.Message);
    }
     
[<EntryPoint>]
 let main _ =
     Async.RunSynchronously(fetchAsync)
     0