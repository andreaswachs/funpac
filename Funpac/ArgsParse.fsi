module ArgsParse

    type ProgramSettings
    type SpecialCommand =
        OpenTemplatesFolder
        | OpenConfig
        | ListEntries
    
    val parseArgs : string array -> ProgramSettings
    val getKey : ProgramSettings -> string option
    val getVerboseMode : ProgramSettings -> bool
    val getSpecialCommand : ProgramSettings -> SpecialCommand option