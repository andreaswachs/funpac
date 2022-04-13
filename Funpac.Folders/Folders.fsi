module Folders

    // Public helpers
    val (+/) : string -> string -> string
     
    // Path operations
    val getRootConfigPath : string
    val getTemplatesFolderPath : string
    
    // Interaction operations
    val ensureAllFoldersCreated : unit