#I @"C:\Users\krata\.nuget\packages\" // TODO reference it from solution folder
#r @"fsharp.data\3.3.3\lib\net45\FSharp.Data.dll"

open FSharp.Data

type Questions = JsonProvider<"""https://api.stackexchange.com/2.2/questions?site=stackoverflow""">

let csQuestions = """https://api.stackexchange.com/2.2/questions?site=stackoverflow&tagged=C%23"""

Questions.Load(csQuestions).Items |> Seq.iter (fun q -> printfn "%s" q.Title)
