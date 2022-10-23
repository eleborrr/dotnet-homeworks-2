open System
open System.Net
open System.Net.Http
open System.Threading.Tasks

let fetchAsync =
    async{
        use client = new HttpClient()
        while true do
            try
                let args = Console.ReadLine().Split()
                if args.Length <> 3 then
                    raise (ArgumentException("wrong arguments length"))
                let uri = new Uri $"https://localhost:5001/calculate?value1={args[0]}&operation={args[1]}&value2={args[2]}"
                let! res = Async.AwaitTask(client.GetStringAsync(uri))
                printfn $"{res}"
            with
                | ex -> printfn "%s" (ex.Message);
    }
     
[<EntryPoint>]
 let main _ =
     Async.RunSynchronously(fetchAsync)
     0