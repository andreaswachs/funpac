module Tests.ArgsParse

open Xunit
open ArgsParse

[<Fact>]
let ``parseArgs given empty list parses no args`` () =
    let args = []
    
    let actual = parseArgs args
    
    Assert.Equal(None, getKey actual)
    Assert.False(getVerboseMode actual)
    
[<Fact>]
let ```arseArgs given flag --verbose sets verbose mode to true`` () =
    let args = ["--verbose"]
    
    let actual = parseArgs args
    
    Assert.Equal(None, getKey actual) // Check that it didn't mess with the key
    Assert.True(getVerboseMode actual)