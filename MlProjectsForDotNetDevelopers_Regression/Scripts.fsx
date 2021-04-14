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

[
    Scatter( x = [ 0 .. count.Length], y = count );
    Scatter( x = [ 0 .. smallWindow.Length], y = smallWindow );
    Scatter( x = [ 0 .. largeWindow.Length], y = largeWindow );
]
|> Chart.Plot
|> Chart.Show