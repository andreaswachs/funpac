module Files

    open System
    open System.IO
    open System.Net.Http
    open Folders

    // See more: https://gist.github.com/andreaswachs/eaf0c7a3b2deb8eb607a3cd83533cfa0
    let templateConfigFileUrl : string =
        "https://gist.githubusercontent.com/andreaswachs/eaf0c7a3b2deb8eb607a3cd83533cfa0/raw/1755c861f795f2c1552343df761eac625f2c4690/funpac-default-config.yaml"
        
    let configFilePath : string =
        rootConfigPath +/ "config.yml"
        
    let fetchOnlineFile (url: string) : Async<string> =
        async {
            use httpClient = new HttpClient()
            let! response = httpClient.GetAsync(url) |> Async.AwaitTask
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
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
        
        
    