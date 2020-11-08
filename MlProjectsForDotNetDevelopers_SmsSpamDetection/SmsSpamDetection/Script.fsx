open System.IO

#load "Domain.fs"
#load "NaiveBayes.fs"

open Domain
open NaiveBayes.Classifier


let datasetPath = Path.Combine(__SOURCE_DIRECTORY__, "Data", "SMSSpamCollection")

let parseDocType (label:string) =
    match label with
    | "ham" -> Ham
    | "spam" -> Spam
    | _ -> failwith "Unknown label"

let parseLine (line:string) =
    let split = line.Split('\t')
    let label = split.[0] |> parseDocType
    let message = split.[1]
    (label, message)

let dataset =
    File.ReadAllLines datasetPath
    |> Array.map parseLine

let spamWithFREE =
    dataset
    |> Array.filter (fun (docType,_) -> docType = Spam)
    |> Array.filter (fun (_,sms) -> sms.Contains("FREE"))
    |> Array.length

let hamWithFREE =
    dataset
    |> Array.filter (fun (docType,_) -> docType = Ham)
    |> Array.filter (fun (_,sms) -> sms.Contains("FREE"))
    |> Array.length

