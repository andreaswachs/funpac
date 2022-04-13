module Funpac.Files

    open System
    open System.IO
    open System.Net.Http
    open Folders
    open Legivel

    // TODO: If needing to fetch the deafult file, also get the related default files


    let templateConfigFileUrl : string =
        "https://raw.githubusercontent.com/andreaswachs/issues-to-md/main/files/1.md"
        
    let configFilePath : string =
        getRootConfigPath +/ "config.yml"
    
    let fetchOnlineFile (url: string) : Async<string> =
        async {
            use httpClient = new HttpClient()
            let! response = httpClient.GetAsync(url) |> Async.AwaitTask
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
        }
    
    let fetchDefaultConfigFile : string =
        fetchOnlineFile templateConfigFileUrl
        |> Async.RunSynchronously
        |> fun s -> s.Replace("\r\n", "_NLTOKEN_")
        |> fun s -> s.Replace("\n", "_NLTOKEN_")
        |> fun s -> s.Replace("_NLTOKEN_", Environment.NewLine)
        
    let createDefaultConfigFile : unit =
        File.WriteAllText(configFilePath, fetchDefaultConfigFile)
    
    let ensureConfigFileExists : unit =
        match File.Exists(configFilePath) with
        | true -> ()
        | false -> createDefaultConfigFile
        
    

