module Funpac.Files

    open System
    open System.IO
    open System.Net.Http
    open Folders
    open System.Text.RegularExpressions

    let templateConfigFileUrl : string =
        "TBD"
        
    let configFilePath : string =
        printf "configFilePath() triggered\n"
        getRootConfigPath +/ "config.yml"
    
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
  type: file
  fileName: file1.txt
- key: template2
  type: files
  fileNames: [file2.txt, file3.txt]
- key: template3
  type: folder
  folderName: folder1"""
        }
    
    let fetchDefaultConfigFile : string =
        printf "fetchDefaultConfigFile() triggered\n"
        fetchOnlineFile templateConfigFileUrl
        |> Async.RunSynchronously
        // This is stupid but I'm very afraid of newlines between OS'
        |> fun s -> s.Replace("\r\n", "_NLTOKEN_")
        |> fun s -> s.Replace("\n", "_NLTOKEN_")
        |> fun s -> s.Replace("_NLTOKEN_", Environment.NewLine)
        
    let createDefaultConfigFile : unit =
        printf "createDefaultConfigFile() triggered\n"
        File.WriteAllText(configFilePath, fetchDefaultConfigFile)
    
    let doEnsureConfigFileExists =
        function
        | false -> createDefaultConfigFile
        | _ -> ()

    let ensureConfigFileExists : unit =
        printf "ensureConfigFileExists() triggered\n"
        File.Exists(configFilePath) |> doEnsureConfigFileExists
        
    let readConfigFile : string =
        printf "readConfigFile() triggered\n"
        """settings:
  unusedSetting: true
  otherUnusedSetting: false
entries:
- key: template1
  type: file
  fileName: file1.txt
- key: template2
  type: files
  fileNames: [file2.txt, file3.txt]
- key: template3
  type: folder
  folderName: folder1"""




