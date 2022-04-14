[<EntryPoint>]
let main(args) =
    Config.setup() |> ignore
    let config = Config.loadConfig()
    let entries = Config.getEntries config
    let settings = ArgsParse.parseArgs args
    
    let key = match ArgsParse.getKey settings with
              | None -> printfn "Error: no template key given"; exit 1
              | Some key -> key
    
    let entry = match Config.entryOfKey key entries with
                | Some entry -> entry
                | None -> printfn $"Error: no matching template key for key \"{key}\""; exit 1
    
    printfn $"You've provided key \"{key}\""
    printfn $"We've got entry for key: \"{entry}\”"
    
    Config.unpack entry
    0
