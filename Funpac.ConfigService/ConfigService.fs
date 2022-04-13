module Funpac.ConfigService

    open Legivel.Serialization
    
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
    
    type Config = Config of YamlConfigTopLevel

    let setup : unit =
        printf "ConfigService.setup() triggered\n"
    
    let loadConfig : DeserializeResult<YamlConfigTopLevel> list =
        Deserialize<YamlConfigTopLevel> Files.readConfigFile

    