open System
open System.IO

module Float =

    let tryFromString s =
        match s with
        | "N/A" -> None
        | _ -> Some (float s)

    let fromStringOr d s =
        s
        |> tryFromString
        |> Option.defaultValue d

type Student =
    {
        Name : string
        Id : string
        MeanScore : float
        MinScore : float
        MaxScore : float
    }

 module Student =

    let fromString (s : string) =
        let elements = s.Split('\t')
        let name = elements.[0]
        let id = elements.[1]
        let scores =
            elements
            |> Array.skip 2
            |> Array.map (Float.fromStringOr 50.0)
        let meanScore = scores |> Array.average
        let minScore = scores |> Array.min
        let maxScore = scores |> Array.max
        {
            Name = name
            Id = id
            MeanScore = meanScore
            MinScore = minScore
            MaxScore = maxScore
        }

    let printSummary (student : Student) =
        printfn "%s\t%s\t%0.1f\t%0.1f\t%0.1f" student.Name student.Id student.MeanScore student.MinScore student.MaxScore

let summerize filePath =
    let rows = File.ReadAllLines filePath
    let studentCount = (rows |> Array.length) - 1
    printfn "Student count %i" studentCount
    rows
    |> Array.skip 1
    |> Array.map Student.fromString
    |> Array.sortBy (fun student -> student.Name)
    |> Array.iter Student.printSummary

[<EntryPoint>]
let main argv =
    let filePath = argv.[0]
    match argv.Length with
    | 1 ->
        match (File.Exists filePath) with
        | true ->
            printfn "Processing %s" filePath
            summerize filePath
            0
        | _ ->
            printfn "File not found: %s" filePath
            2
    | _ ->
        printfn "Please specify a file."
        1