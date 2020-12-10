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

