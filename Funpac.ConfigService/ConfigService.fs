module Funpac.ConfigService

    let setup : unit =
        printf "Hello World, we're going to ensure everything is in place."
        Folders.ensureAllFoldersCreated
        Files.ensureConfigFileExists

