namespace StudentScores

module Summary =

    open System.IO

    let summerize filePath =
        let rows = File.ReadAllLines filePath
        let studentCount = (rows |> Array.length) - 1
        printfn "Student count %i" studentCount
        rows
        |> Array.skip 1
        |> Array.map Student.fromString
        |> Array.sortBy (fun student -> student.Name)
        |> Array.iter Student.printSummary
