#r "nuget: FSharp.Data"
#r "nuget: XPlot.Plotly"

open System
open System.IO
open FSharp.Data
open XPlot.Plotly

let folder = __SOURCE_DIRECTORY__
let file = "userprofiles-toptags.txt"

let headers, observations =
    let raw =
        Path.Combine(folder, file)
        |> File.ReadAllLines
    
    let headers = (raw.[0].Split ',').[1..]

    let observations =
        raw.[1..]
        |> Array.map (fun line -> (line.Split ',').[1..])
        |> Array.map (Array.map float)

    headers, observations

printfn "%16s %8s %8s %8s" "Tag Name" "Avg" "Min" "Max"
headers
|> Array.iteri (fun i name ->
   let col = observations |> Array.map (fun obs -> obs.[i])
   let avg = col |> Array.average
   let min = col |> Array.min
   let max = col |> Array.max
   printfn "%16s %8.1f %8.1f %8.1f" name avg min max)

headers
|> Seq.mapi (fun i name ->
    name,
    observations
    |> Seq.averageBy (fun obs -> obs.[i]))
|> Chart.Bar
|> Chart.Show

#load "KMeans.fs"
open Unsupervised.KMeans

type Observation = float []

let features = headers.Length

let distance (obs1:Observation) (obs2:Observation) =
    (obs1, obs2) 
    ||> Seq.map2 (fun u1 u2 -> pown (u1 - u2) 2)
    |> Seq.sum
    |> sqrt

let centroidOf (cluster:Observation seq) =
    Array.init features (fun f ->
        cluster 
        |> Seq.averageBy (fun user -> user.[f]))

let observations1 = 
    observations 
    |> Array.map (Array.map float) 
    |> Array.filter (fun x -> Array.sum x > 0.)

let (clusters1, classifier1) = 
    let clustering = clusterize distance centroidOf
    let k = 5
    clustering observations1 k
