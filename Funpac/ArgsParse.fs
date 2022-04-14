module ArgsParse

    open System.IO

    type SpecialCommand =
        OpenTemplatesFolder
        | OpenConfig
    
    type ProgramSettings = {
        mutable key: string option
        mutable verbose : bool
        mutable specialCommand : SpecialCommand option
    }
    
    let emptyProgramSettings () : ProgramSettings =
        { key = None
          verbose = false
          specialCommand = None
        }
        
    let getKey (settings: ProgramSettings) : string option =
        settings.key
        
    let getVerboseMode (settings: ProgramSettings) : bool =
        settings.verbose
        
    let getSpecialCommand (settings: ProgramSettings) : SpecialCommand option =
        settings.specialCommand
        
    let printHelp =
        let txt = """
Usage: funpac [options] [key]

Options:
  -h|--help         Displays help
  -v|--verbose      Activates verbose output mode (currently unsupported)
  --version         Prints the version
  --config          Prints the location of the config file
  --templates       Prints the location of the templates folder

Key: The template key, which is searched for the the config file"""
        printfn "%s" txt
    
    let parseArgs (args: string array) =
        let rec aux (settings: ProgramSettings) (arg: string) : ProgramSettings =
            match arg with
            | "--help" | "-h" ->
                printHelp
                exit 0
            | "--verbose" | "-v" ->
                settings.verbose <- true
                printfn $"Note: verbose mode not supported as of version {Meta.version}."
            | "--version" ->
                printfn $"funpac version: {Meta.version}"
                exit 0
            | "--templates" ->
                settings.specialCommand <- Some OpenTemplatesFolder
            | "--config" ->
                settings.specialCommand <- Some OpenConfig
            | key ->
                settings.key <- Some key
                
            settings
        Array.fold aux (emptyProgramSettings()) args
        
        
    
        


