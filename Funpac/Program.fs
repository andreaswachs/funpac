open Funpac
open Funpac.ConfigService


// ConfigService.setup
let c = ConfigService.loadConfig

printf "The config is as follows:\n%A\n" c