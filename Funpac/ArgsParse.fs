module ArgsParse

    type ProgramSettings = {
        mutable key: string option
        mutable verbose : bool
    }
    
    let emptyProgramSettings () : ProgramSettings =
        { key = None
          verbose = false }
    
    let parseArgs (args: string array) =
        let rec aux (settings: ProgramSettings) (arg: string) : ProgramSettings =
            match arg with
            | "--verbose" ->
                settings.verbose <- true    
            | key ->
                settings.key <- Some key
                
            settings
        Array.fold aux (emptyProgramSettings()) args
        
        
    let getKey (settings: ProgramSettings) : string option =
        settings.key
        
    let getVerboseMode (settings: ProgramSettings) : bool =
        settings.verbose
        

