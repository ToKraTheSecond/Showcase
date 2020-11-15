// Learn more about F# at http://fsharp.org

open System
open System.IO

open Domain
open Operations
open Tokenizer

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

let ham,spam =
    let rawHam,rawSpam =
        training
        |> Array.partition (fun (lbl,_) -> lbl=Ham)
    rawHam |> Array.map snd,
    rawSpam |> Array.map snd

[<EntryPoint>]
let main argv =
    let hamCount = ham |> vocabulary casedTokenizer |> Set.count
    let spamCount = spam |> vocabulary casedTokenizer |> Set.count
    
    let topHam = ham |> top (hamCount / 5) casedTokenizer
    let topSpam = spam |> top (spamCount / 20) casedTokenizer

    let topTokens = Set.union topHam topSpam
    let commonTokens = Set.intersect topHam topSpam
    let specificTokens = Set.difference topTokens commonTokens
    let smartTokens =
        specificTokens
        |> Set.add "__TXT__"
        |> Set.add "__PHONE__"    
    
    for l in 10 .. 10 .. 130 do printfn "P(Spam if Length > %i) = %.4f" l (lengthAnalysis l dataset)
    
    let bestClassifier = train training smartTokenizer smartTokens
    validation
    |> Seq.filter (fun (docType,_) -> docType = Ham)
    |> Seq.averageBy (fun (docType,sms) ->
        if docType = bestClassifier sms
        then 1.0
        else 0.0)
    |> printfn "Properly classified Ham: %.5f"
    validation
    |> Seq.filter (fun (docType,_) -> docType = Spam)
    |> Seq.averageBy (fun (docType,sms) ->
        if docType = bestClassifier sms
        then 1.0
        else 0.0)
    |> printfn "Properly classified Spam: %.5f"

    let evaluate (tokenizer:Tokenizer) (tokens:Token Set) (training:(_ * string) []) (validation:(_ * string) []) =
        let classifier = train training tokenizer tokens
        validation
        |> Seq.averageBy (fun (docType,sms) -> if docType = classifier sms then 1.0 else 0.0)

    let result = evaluate smartTokenizer smartTokens training validation
    
    printfn "Correctly classified: %.3f" result

    
    0