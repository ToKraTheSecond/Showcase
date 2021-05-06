open System
open System.IO

open Operations


[<EntryPoint>]
let main argv =
    let trainingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "trainingsample.csv");
    let trainingData = reader trainingPath
    
    let validationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "validationsample.csv");
    let validationData = reader validationPath  
    
    let manhattanClassifier = train trainingData manhattanDistance
    let euclideanClassifier = train trainingData euclideanDistance
    
    printfn "Manhattan"
    evaluate validationData manhattanClassifier
    printfn "Euclidean"
    evaluate validationData euclideanClassifier

    0
