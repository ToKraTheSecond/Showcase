open System
open System.IO
open StudentScores

[<EntryPoint>]
let main argv =
    let filePath = argv.[0]
    match argv.Length with
    | 1 ->
        match (File.Exists filePath) with
        | true ->
            printfn "Processing %s" filePath
            Summary.summerize filePath
            0
        | _ ->
            printfn "File not found: %s" filePath
            2
    | _ ->
        printfn "Please specify a file."
        1