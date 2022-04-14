module Config

    open System
    open System.IO
    open Folders
    open Files
    open Legivel.Serialization
    
    
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Types
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    
    type YamlSettings = {
        unusedSetting: bool option
        otherUnusedSetting: bool option
    } 
    type YamlEntry = {
        key: string
        fileNames: string list option
        folderNames: string list option
    }
    type YamlConfigTopLevel = {
        settings: YamlSettings option
        entries: YamlEntry list option
    }
    type Config = Config of YamlConfigTopLevel
    
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Functions for folders and files
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    
    let setup () : unit =
        Folders.ensureAllFoldersCreated()
        Files.ensureConfigFileExists()
    
    let loadConfig () : DeserializeResult<YamlConfigTopLevel> =
        Files.readConfigFile()
        |> Deserialize<YamlConfigTopLevel>
        |> List.head
        
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Helper functions for extracting data
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

    let getSuccess (config: DeserializeResult<YamlConfigTopLevel>) =
        match config with
        | Success config' -> config'
        | Error _ -> failwith "Yaml configuration was not successfully parsed"
        
    let getDataFromSuccess (config: DeserializeResult<YamlConfigTopLevel>) =
        // This is horrible and I like it
        (fun c -> c.Data) << getSuccess <| config
        

    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Functions for working with entries
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    
    let getEntries (config: DeserializeResult<YamlConfigTopLevel>) =
        let data = getDataFromSuccess config
        match data.entries with
        | Some lst -> lst
        | None -> []
    
    let entryOfKey (key: string) (entries: YamlEntry list) : YamlEntry option =
        List.tryFind (fun entry -> entry.key = key) entries
        
    let filesOfEntry (entry: YamlEntry) : string list =
        match entry.fileNames with
        | Some fileNames -> fileNames
        | None -> []
        
    let foldersOfEntry (entry: YamlEntry) : string list =
        match entry.folderNames with
        | Some folderNames -> folderNames
        | None -> []
        
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Functions unpacks templates
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    
    let filenameFromFullPath (destination: string) (file: string) : string =
        file.Split(Path.DirectorySeparatorChar) |> Array.last |> fun f -> Path.Combine(destination, f)
    
    let unpackFiles (entry: YamlEntry) : unit =
        let destination = Directory.GetCurrentDirectory()
        
        for file in filesOfEntry entry do
            let fileOut = filenameFromFullPath destination file
            copyTemplateFile fileOut file
            
    let unpackFolders (entry: YamlEntry) : unit =
        let destination = Directory.GetCurrentDirectory()
        
        for folder in foldersOfEntry entry do
            let destination', files = copyFolder destination folder
            for file in files do
                let fileOut = filenameFromFullPath destination' file 
                Files.copyFile fileOut file
            
    let unpack (entry: YamlEntry) : unit =
        unpackFolders entry |> ignore
        unpackFiles entry |> ignore