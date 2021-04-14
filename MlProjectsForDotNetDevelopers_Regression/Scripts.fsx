#r "nuget: FSharp.Data"
#r "nuget: XPlot.Plotly"
// #r "nuget: XPlot.GoogleCharts"

open FSharp.Data
open XPlot.Plotly
// open XPlot.GoogleCharts

[<Literal>]
let dataPath = __SOURCE_DIRECTORY__ + "/Data/day.csv"

let ma n (series:float seq) =
    series
    |> Seq.windowed n
    |> Seq.map (fun xs -> xs |> Seq.average)
    |> Seq.toList

type Data = CsvProvider<dataPath>
let dataset = Data.Load(dataPath)
let data = dataset.Rows
let count = [ for obs in data -> obs.Cnt |> float ]
let smallWindow = ma 7 count
let largeWindow = ma 30 count

let xAxisRange = [ 0 .. count.Length]

[
    Scatter( x = xAxisRange, y = count );
    Scatter( x = xAxisRange, y = smallWindow );
    Scatter( x = xAxisRange, y = largeWindow );
]
|> Chart.Plot
|> Chart.Show

let baseline =
    let avg =
        data
        |> Seq.averageBy (fun x -> float x.Cnt)
    data |> Seq.averageBy (fun x -> abs (float x.Cnt - avg))

type Obs = Data.Row
type Model = Obs -> float

let model (theta0, theta1) (obs:Obs) = theta0 + theta1 * (float obs.Instant)

let model0 = model (4504., 0.)
let model1 = model (6000., -4.5)

[
    Scatter ( x = xAxisRange, y = count );
    Scatter ( x = xAxisRange, y = [for obs in data -> model0 obs]);
    Scatter ( x = xAxisRange, y = [for obs in data -> model1 obs]);
]
|> Chart.Plot
|> Chart.Show

let cost (data:Obs seq) (m:Model) =
    data
    |> Seq.sumBy (fun x -> pown (float x.Cnt - m x) 2)
    |> sqrt

let overallCost = cost data
overallCost model0 |> printfn "Cost model0: %.0f"
overallCost model1 |> printfn "Cost model1: %.0f"

let update alpha (theta0, theta1) (obs:Obs) =
    let y = float obs.Cnt
    let x = float obs.Instant
    let theta0' = theta0 - 2. * alpha * 1. * (theta0 + theta1 * x - y)
    let theta1' = theta1 - 2. * alpha * x * (theta0 + theta1 * x - y)
    theta0', theta1'

let obs100 = data |> Seq.nth 100
let testUpdate = update 0.00001 (0.,0.) obs100
cost [obs100] (model (0.,0.))
cost [obs100] (model testUpdate)

let stochastic rate (theta0,theta1) =
    data
    |> Seq.fold (fun (t0,t1) obs ->
        printfn "%.4f, %.4f" t0 t1
        update rate (t0,t1) obs) (theta0,theta1)

let tune_rate =
    [ for r in 1 .. 20 -> (pown 0.1 r), stochastic (pown 0.1 r) (0.,0.) |> model |> overallCost ]

let rate = pown 0.1 8
let model2 = model (stochastic rate (0.0,0.0))

[
    Scatter ( x = xAxisRange, y = count );
    Scatter ( x = xAxisRange, y = [for obs in data -> model2 obs]);
]
|> Chart.Plot
|> Chart.Show

let hiRate = 10.0 * rate
let error_eval =
    data
    |> Seq.scan (fun (t0,t1) obs -> update hiRate (t0,t1) obs) (0.0,0.0)
    |> Seq.map (model >> overallCost)
    |> Chart.Line
    |> Chart.Show