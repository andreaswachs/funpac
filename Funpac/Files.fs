module Files

    open System
    open System.IO
    open System.Net.Http
    open Folders

    let templateConfigFileUrl : string =
        "TBD"
        
    let configFilePath : string =
        rootConfigPath +/ "config.yml"
        
    let fetchOnlineFile (url: string) : Async<string> =
        // async {
        //     use httpClient = new HttpClient()
        //     let! response = httpClient.GetAsync(url) |> Async.AwaitTask
        //     let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        //     return content
        // }
        async {
            return """settings:
  unusedSetting: true
  otherUnusedSetting: false
entries:
  - key: template1
    fileNames: [file1.txt]
  - key: template2
    fileNames: [file2.txt, file3.txt]
  - key: template3
    folderNames: [folder1]"""
        }

    let fetchDefaultConfigFile () : string =
        fetchOnlineFile templateConfigFileUrl
        |> Async.RunSynchronously
        |> fun s -> s.Replace("\r\n", "_NLTOKEN_")
        |> fun s -> s.Replace("\n", "_NLTOKEN_")
        |> fun s -> s.Replace("_NLTOKEN_", Environment.NewLine)
        
    let createDefaultConfigFile () : unit =
        File.WriteAllText(configFilePath, fetchDefaultConfigFile())

    let ensureConfigFileExists () : unit =
        match File.Exists(configFilePath) with
        | true -> ()
        | false -> createDefaultConfigFile()
        
    let readConfigFile () : string =
        File.ReadAllText configFilePath
        
    let copyFile (destination: string) (source: string) =
        File.ReadAllText(source) |> fun content -> File.WriteAllText(destination, content)
    
    let copyTemplateFile (destination: string) (file: string) =
        // We assume that 'file' variable contains the path from the template directory
        pathToTemplateFile file |> copyFile destination
        
        
    