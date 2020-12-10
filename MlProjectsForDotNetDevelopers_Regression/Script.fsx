#I @"C:\Users\krata\.nuget\packages\" // TODO reference it from solution folder
#r @"fsharp.data\3.3.3\lib\net45\FSharp.Data.dll"
#load @"fsharp.charting\2.1.0\FSharp.Charting.fsx"

open FSharp.Data
open FSharp.Charting

type Data = CsvProvider<"Data/day.csv">
let dataset = Data.Load("Data/day.csv")
let data = dataset.Rows
let count = data |> Seq.map (fun x -> float x.Cnt)

let ma n (series:float seq) =
    series
    |> Seq.windowed n
    |> Seq.map (fun xs -> xs |> Seq.average)
    |> Seq.toList

Chart.Combine [
    Chart.Line count
    Chart.Line (ma 7 count)
    Chart.Line (ma 30 count) ]

// Define baseline
let baseline =
    let avg = data |> Seq.averageBy (fun x -> float x.Cnt)
    data |> Seq.averageBy (fun x -> abs (float x.Cnt - avg))

// Define a basic straight line mode
type Obs = Data.Row

let model (theta0, theta1) (obs:Obs) =
    theta0 + theta1 * (float obs.Instant)

let model0 = model (4504., 0.)
let model1 = model (6000., -4.5)

Chart.Combine [
    Chart.Line count
    Chart.Line [ for obs in data -> model0 obs ]
    Chart.Line [ for obs in data -> model1 obs ] ]

// Finding the lowest cost model
type Model = Obs -> float

let cost (data:Obs seq) (m:Model) =
    data
    |> Seq.sumBy (fun x -> pown (float x.Cnt - m x) 2)
    |> sqrt

let overallCost = cost data
overallCost model0 |> printfn "Cost model0: %.0f"
overallCost model1 |> printfn "Cost model1: %.0f"

// Finding the minimum of a function with gradient descent
// Stochastic Gradient Descent

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
        printfn "%.4f,%.4f" t0 t1
        update rate (t0,t1) obs) (theta0, theta1)

let tune_rate =
    [ for r in 1 .. 20 ->
        (pown 0.1 r), stochastic (pown 0.1 r) (0.,0.) |> model |> overallCost ]

let rate = pown 0.1 8
let model2 = model (stochastic rate (0.0,0.0))

Chart.Combine [
    Chart.Line count
    Chart.Line [ for obs in data -> model2 obs ]]