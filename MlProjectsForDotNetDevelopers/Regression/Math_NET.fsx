#r "nuget: FSharp.Data"
#r "nuget: XPlot.Plotly"
#r "nuget: MathNet.Numerics.FSharp"

open FSharp.Data
open XPlot.Plotly
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

let A = vector [ 1.; 2.; 3. ]
let B = matrix [
            [ 1.; 2. ]
            [ 3.; 4. ]
            [ 5.; 6. ] 
        ]

let C = A * A
let D = A * B
let E = A * B.Column(1)

[<Literal>]
let dataPath = __SOURCE_DIRECTORY__ + "/Data/day.csv"
type Data = CsvProvider<dataPath>
let dataset = Data.Load(dataPath)
let data = dataset.Rows

type Vec = Vector<float>
type Mat = Matrix<float>

type Obs = Data.Row
type Model = Obs -> float
type Featurizer = Obs -> float list

let update alpha (theta0, theta1) (obs:Obs) =
    let y = float obs.Cnt
    let x = float obs.Instant
    let theta0' = theta0 - 2. * alpha * 1. * (theta0 + theta1 * x - y)
    let theta1' = theta1 - 2. * alpha * x * (theta0 + theta1 * x - y)
    theta0', theta1'

let cost (theta:Vec) (Y:Vec) (X:Mat) =
    let ps = Y - (theta * X.Transpose())
    ps * ps |> sqrt

let predict (theta:Vec) (v:Vec) = theta * v

let X = matrix [ for obs in data -> [ 1.; float obs.Instant ]]
let Y = vector [ for obs in data -> float obs.Cnt ]

let theta = vector [6000.; -4.5]

predict theta (X.Row(0))
cost theta Y X

let estimate (Y:Vec) (X:Mat) = (X.Transpose() * X).Inverse() * X.Transpose() * Y

let seed = 314159
let rng = System.Random(seed)

// Fisher-Yates shuffle
let shuffle (arr:'a []) =
    let arr = Array.copy arr
    let l = arr.Length
    for i in (l-1) .. -1 .. 1 do
        let temp = arr.[i]
        let j = rng.Next(0,i+1)
        arr.[i] <- arr.[j]
        arr.[j] <- temp
    arr

let training,validation =
    let shuffled =
        data
        |> Seq.toArray
        |> shuffle
    let size = 0.7 * float (Array.length shuffled) |> int
    shuffled.[..size],
    shuffled.[size+1..]

let exampleFeaturizer (obs:Obs) = [ 1.0; float obs.Instant; ]
let predictor (f:Featurizer) (theta:Vec) = f >> vector >> (*) theta

let evaluate (model:Model) (data:Obs seq) =
    data
    |> Seq.averageBy (fun obs ->
        abs (model obs - float obs.Cnt))

let model (f:Featurizer) (data:Obs seq) =
    let Yt, Xt =
        data
        |> Seq.toList
        |> List.map (fun obs -> float obs.Cnt, f obs)
        |> List.unzip
    let theta = estimate (vector Yt) (matrix Xt)
    let predict = predictor f theta
    theta,predict

let featurizer0 (obs:Obs) = [ 1.; float obs.Instant; ]
let (theta0,model0) = model featurizer0 training

evaluate model0 training |> printfn "Training: %.0f"
evaluate model0 validation |> printfn "Validation: %.0f"

let featurizer1 (obs:Obs) =
    [
        1.
        obs.Instant |> float
        obs.Atemp |> float
        obs.Hum |> float
        obs.Temp |> float
        obs.Windspeed |> float
    ]

let (theta1,model1) = model featurizer1 training

evaluate model1 training |> printfn "Training: %.0f"
evaluate model1 validation |> printfn "Validation: %.0f"

let xAxisRange = [ 0 .. [ for obs in data -> float obs.Cnt ].Length]

[
    Scatter( x = xAxisRange, y = [ for obs in data -> float obs.Cnt ] );
    Scatter( x = xAxisRange, y = [ for obs in data -> model0 obs ] );
    Scatter( x = xAxisRange, y = [ for obs in data -> model1 obs ]);
]
|> Chart.Plot
|> Chart.Show

[ for obs in data -> float obs.Cnt, model1 obs ]
|> Chart.Scatter
|> Chart.Show

let featurizer2 (obs:Obs) =
    [
        1.
        obs.Instant |> float
        obs.Hum |> float
        obs.Temp |> float
        obs.Windspeed |> float
        (if obs.Weekday = 1 then 1.0 else 0.0)
        (if obs.Weekday = 2 then 1.0 else 0.0)
        (if obs.Weekday = 3 then 1.0 else 0.0)
        (if obs.Weekday = 4 then 1.0 else 0.0)
        (if obs.Weekday = 5 then 1.0 else 0.0)
        (if obs.Weekday = 6 then 1.0 else 0.0)
    ]

let (theta2,model2) = model featurizer2 training

evaluate model2 training |> printfn "Training: %.0f"
evaluate model2 validation |> printfn "Validation: %.0f"