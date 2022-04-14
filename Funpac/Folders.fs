module Folders
    open System
    open System.IO

    /// <summary>
    /// Combines two paths. Used to create paths more idiomatic in F#
    /// </summary>
    /// <param name="path1">The leftmost path</param>
    /// <param name="path2">The path to concatenate onto the right</param>
    /// <returns>The concatenated path with correct folder separator</returns>
    let (+/) path1 path2 = Path.Combine(path1, path2)


    /// <summary>
    /// Gets the path to the root of the configuration folder
    /// </summary>
    /// <returns>The absolute path to the <i>funpac</i> directory</returns>
    let rootConfigPath : string =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +/ "funpac"

    /// <summary>
    /// Gets the path to the location of the template files folder
    /// </summary>
    let templatesFolderPath : string =
        rootConfigPath +/ "templates"
        
    let pathToTemplateFile (localFilePath: string) : string =
        templatesFolderPath +/ localFilePath
        
    /// <summary>
    /// Ensures that all folders are created
    /// </summary>
    let ensureAllFoldersCreated () : unit =
        try
            Directory.CreateDirectory templatesFolderPath |> ignore
        with
        | Failure(msg) -> failwith msg
        
    
    let copyFolder (destinationRoot: string) (subfolderName: string) : (string * string[]) =
        try
            let fullDestination = destinationRoot +/ subfolderName
            // Create folder in destination directory
            fullDestination |> Directory.CreateDirectory |> ignore
            
            templatesFolderPath +/ subfolderName
            |> Directory.GetFiles
            |> fun files -> (fullDestination, files)
            
            
        with
        | Failure(msg) -> failwith msg