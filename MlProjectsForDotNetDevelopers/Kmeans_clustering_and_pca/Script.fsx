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

clusters1
|> Seq.iter (fun (id,profile) ->
    printfn "CLUSTER %i" id
    profile
    |> Array.iteri (fun i value -> printfn "%16s %.1f" headers.[i] value))

observations1
|> Seq.countBy (fun obs -> classifier1 obs)
|> Seq.iter (fun (clusterID,count) ->
    printfn "Cluster %i: %i elements" clusterID count)

let rowNormalizer (obs:Observation) : Observation =
    let max = obs |> Seq.max
    obs |> Array.map (fun tagUse -> tagUse / max)

let observations2 =
    observations
    |> Array.filter (fun x -> Array.sum x > 0.)
    |> Array.map (Array.map float)
    |> Array.map rowNormalizer

let (clusters2, classifier2) =
    let clustering = clusterize distance centroidOf
    let k = 5
    clustering observations2 k

observations2
|> Seq.countBy (fun obs -> classifier2 obs)
|> Seq.iter (fun (clusterID, count) ->
    printfn "Cluster %i: %i elements" clusterID count)

let ruleOfThumb (n:int) = sqrt (float n / 2.)
let k_ruleOfThumb = ruleOfThumb observations2.Length

let squareError (obs1:Observation) (obs2:Observation) =
    (obs1,obs2)
    ||> Seq.zip
    |> Seq.sumBy (fun (x1,x2) -> pown (x1-x2) 2)

let RSS (dataset:Observation[]) centroids =
    dataset
    |> Seq.sumBy (fun obs -> 
        centroids
        |> Seq.map (squareError obs)
        |> Seq.min)

let AIC (dataset:Observation[]) centroids =
    let k = centroids |> Seq.length
    let m = dataset.[0] |> Seq.length
    RSS dataset centroids + float (2 * m * k)

[1..25]
|> Seq.map (fun k ->
    let value =
        [ for _ in 1 .. 10 -> 
            let (clusters, classifier) =
                let clustering = clusterize distance centroidOf
                clustering observations2 k
            AIC observations2 (clusters |> Seq.map snd)]
        |> List.average
    k, value)
|> Chart.Line
|> Chart.Show