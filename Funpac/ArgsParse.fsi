module ArgsParse

    type ProgramSettings
    
    val parseArgs : string array -> ProgramSettings
    val getKey : ProgramSettings -> string option
    val getVerboseMode : ProgramSettings -> bool