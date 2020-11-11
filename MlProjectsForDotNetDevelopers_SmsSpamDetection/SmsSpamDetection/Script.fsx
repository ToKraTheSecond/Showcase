open System.IO

#load "Domain.fs"
#load "NaiveBayes.fs"
#load "Operations.fs"

open Domain
open NaiveBayes.Classifier
open Operations


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

let validation = dataset.[0 .. 999]
let training = dataset.[1000 ..]

(*
let txtClassifier = train training tokens (["txt"] |> set)

validation
|> Seq.averageBy (fun (docType,sms) -> if docType = txtClassifier sms then 1.0 else 0.0)
|> printfn "Based on 'txt', correctly classified: %.3f"
*)

let vocabulary (tokenizer:Tokenizer) (corpus:string seq) =
    corpus
    |> Seq.map tokenizer
    |> Set.unionMany

let allTokens =
    training
    |> Seq.map snd
    |> vocabulary tokens

let evaluate (tokenizer:Tokenizer) (tokens:Token Set) =
    let classifier = train training tokenizer allTokens
    validation
    |> Seq.averageBy (fun (docType,sms) -> if docType = classifier sms then 1.0 else 0.0)
    |> printfn "Correctly classified: %.3f"

evaluate tokens allTokens