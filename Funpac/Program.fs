[<EntryPoint>]
let main(args) =
    Config.setup() |> ignore
    let settings = ArgsParse.parseArgs args
    let config = Config.loadConfig()
    let entries = Config.getEntries config
    
    ArgsParse.getSpecialCommand settings |> Config.doSpecialCommand entries
    
    let key = match ArgsParse.getKey settings with
              | None -> printfn "Error: no template key given"; exit 1
              | Some key -> key
    
    let entry = match Config.entryOfKey key entries with
                | Some entry -> entry
                | None -> printfn $"Error: no matching template key for key \"{key}\""; exit 1
    
    Config.unpack entry
    0
