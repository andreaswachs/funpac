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
    let getRootConfigPath : string =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +/ "funpac"

    /// <summary>
    /// Gets the path to the location of the template files folder
    /// </summary>
    let getTemplatesFolderPath : string =
        getRootConfigPath +/ "templates"
        
    
    /// <summary>
    /// Ensures that all folders are created
    /// </summary>
    let ensureAllFoldersCreated : unit =
        try
            printf "Creating full directory: %s\n" getTemplatesFolderPath
            Directory.CreateDirectory getTemplatesFolderPath |> ignore
        with
        | Failure(msg) -> failwith msg
        
        
        
    