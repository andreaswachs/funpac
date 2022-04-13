module Funpac.ConfigService

    open Legivel.Serialization
    
    type Config
    type YamlSettings = {
        unusedSetting: bool
        otherUnusedSetting: bool
    }
    
    type YamlEntries = {
        key: string
        fileNames: string list option
        folderNames: string list option
    }
    
    type YamlConfigTopLevel = {
        settings: YamlSettings option
        entries: YamlEntries list option
    }
    /// Will make sure that required folders and files will be present.
    val setup : unit
    val loadConfig : DeserializeResult<YamlConfigTopLevel> list



