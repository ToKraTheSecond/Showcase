open Operations

[<EntryPoint>]
let main argv =
    let trainingPath = "data/trainingsample.csv"
    let trainingData = reader trainingPath
    
    let validationPath = "data/validationsample.csv"
    let validationData = reader validationPath  
    
    let manhattanClassifier = train trainingData manhattanDistance
    let euclideanClassifier = train trainingData euclideanDistance
    
    printfn "Manhattan"
    evaluate validationData manhattanClassifier
    printfn "Euclidean"
    evaluate validationData euclideanClassifier

    0
