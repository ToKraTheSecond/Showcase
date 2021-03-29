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
            try
                Summary.summerize filePath
                0
            with
            | :? FormatException as e ->
                printfn "Error: %s" e.Message
                printfn "The file was not in the expected format."
                1
            | :? IOException as e ->
                printfn "Error: %s" e.Message
                printfn "The file is open in another program, please close it."
                2
            | _ as e ->
                printfn "Unexpected error: %s" e.Message
                3
        | _ ->
            printfn "File not found: %s" filePath
            4
    | _ ->
        printfn "Please specify a file."
        5