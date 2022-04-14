module Config

    open System.IO
    open ArgsParse
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
    type YamlTemplateEntry = {
        key: string
        files: string list option
        folders: string list option
    }
    type YamlConfigTopLevel = {
        settings: YamlSettings option
        templates: YamlTemplateEntry list option
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
        match data.templates with
        | Some lst -> lst
        | None -> []
    
    let entryOfKey (key: string) (entries: YamlTemplateEntry list) : YamlTemplateEntry option =
        List.tryFind (fun entry -> entry.key = key) entries
        
    let filesOfEntry (entry: YamlTemplateEntry) : string list =
        match entry.files with
        | Some fileNames -> fileNames
        | None -> []
        
    let foldersOfEntry (entry: YamlTemplateEntry) : string list =
        match entry.folders with
        | Some folderNames -> folderNames
        | None -> []
        
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Functions unpacks templates
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    
    let filenameFromFullPath (destination: string) (file: string) : string =
        file.Split(Path.DirectorySeparatorChar) |> Array.last |> fun f -> Path.Combine(destination, f)
    
    let unpackFiles (entry: YamlTemplateEntry) : unit =
        let destination = Directory.GetCurrentDirectory()
        
        for file in filesOfEntry entry do
            let fileOut = filenameFromFullPath destination file
            copyTemplateFile fileOut file
            
    let unpackFolders (entry: YamlTemplateEntry) : unit =
        let destination = Directory.GetCurrentDirectory()
        
        for folder in foldersOfEntry entry do
            let destination', files = copyFolder destination folder
            for file in files do
                let fileOut = filenameFromFullPath destination' file 
                Files.copyFile fileOut file
            
    let unpack (entry: YamlTemplateEntry) : unit =
        unpackFolders entry |> ignore
        unpackFiles entry |> ignore
        
        
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // * Functions that deal with special actions´
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    
    let doSpecialCommand (entries: YamlTemplateEntry list) : SpecialCommand option -> unit =
        function
        | None -> ()
        | Some action ->
             match action with
             | OpenConfig ->
                 printfn "%s" <| Files.configFilePath
             | OpenTemplatesFolder ->
                 printfn "%s" <| Folders.templatesFolderPath
             | ListEntries ->
                printfn "Available templates:"
                for entry in entries do
                    printfn "- %s" entry.key
             exit 0