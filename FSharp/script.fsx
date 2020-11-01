open System.IO

type Observation = { Label:string; Pixels:int[] }

let toObservation (csvData:string) =
    let columns = csvData.Split(',')
    let label = columns.[0]
    let pixels = columns.[1..] |> Array.map int
    { Label = label; Pixels = pixels }

let reader path =
    let data = File.ReadAllLines path
    data.[1..] |> Array.map toObservation

let trainingPath = @"C:\Users\krata\git\FsharpShowcase\MlProjectsForDotNetDevelopers_DigitRecognizer\data\trainingsample.csv" // update this path
let trainingData = reader trainingPath

let validationPath = @"C:\Users\krata\git\FsharpShowcase\MlProjectsForDotNetDevelopers_DigitRecognizer\data\validationsample.csv" // update this path
let validationData = reader validationPath

type Distance = int[] * int[] -> int

let manhattanDistance (pixels1,pixels2) =
    Array.zip pixels1 pixels2
    |> Array.map (fun (x,y) -> abs (x-y))
    |> Array.sum

let euclideanDistance (pixels1, pixels2) =
    Array.zip pixels1 pixels2
    |> Array.map (fun (x,y) -> pown (x-y) 2)
    |> Array.sum

let train (trainingData:Observation[]) (dist:Distance) =
    let classify (pixels:int[]) =
        trainingData
        |> Array.minBy (fun x -> dist (x.Pixels, pixels))
        |> fun x -> x.Label
    classify

let evaluate validationData classifier =
    validationData
    |> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct: %.3f"

let manhattanClassifier = train trainingData manhattanDistance
let euclideanClassifier = train trainingData euclideanDistance

printfn "Manhattan"
evaluate validationData manhattanClassifier
printfn "Euclidean"
evaluate validationData euclideanClassifier

