open System.IO
open System.Text.RegularExpressions

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

let ham,spam =
    let rawHam,rawSpam =
        training
        |> Array.partition (fun (lbl,_) -> lbl=Ham)
    rawHam |> Array.map snd,
    rawSpam |> Array.map snd

let hamCount = ham |> vocabulary casedTokenizer |> Set.count
let spamCount = spam |> vocabulary casedTokenizer |> Set.count

let topHam = ham |> top (hamCount / 10) casedTokenizer
let topSpam = spam |> top (spamCount / 10) casedTokenizer

let topTokens = Set.union topHam topSpam

let allTokens =
    training
    |> Seq.map snd
    |> vocabulary wordTokenizer

let casedTokens =
    training
    |> Seq.map snd
    |> vocabulary casedTokenizer

let evaluate (tokenizer:Tokenizer) (tokens:Token Set) =
    let classifier = train training tokenizer tokens
    validation
    |> Seq.averageBy (fun (docType,sms) -> if docType = classifier sms then 1.0 else 0.0)
    |> printfn "Correctly classified: %.3f"

let commonTokens = Set.intersect topHam topSpam
let specificTokens = Set.difference topTokens commonTokens

let rareTokens n (tokenizer:Tokenizer) (docs:string []) =
    let tokenized = docs |> Array.map tokenizer
    let tokens = tokenized |> Set.unionMany
    tokens
    |> Seq.sortBy (fun t -> countIn tokenized t)
    |> Seq.take n
    |> Set.ofSeq

let rareHam = ham |> rareTokens 50 casedTokenizer |> Seq.iter (printfn "%s")
let rareSpam = spam |> rareTokens 50 casedTokenizer |> Seq.iter (printfn "%s")

let phoneWords = Regex(@"0[7-9]\d{9}")
let phone (text:string) =
    match (phoneWords.IsMatch text) with
    | true -> "__PHONE__"
    | false -> text

let txtCode = Regex(@"\b\d{5}\b")
let txt (text:string) =
    match (txtCode.IsMatch text) with
    | true -> "__TXT__"
    | false -> text

let smartTokens =
    specificTokens
    |> Set.add "__TXT__"
    |> Set.add "__PHONE__"

let smartTokenizer = casedTokenizer >> Set.map phone >> Set.map txt

let lengthAnalysis len =
    let long (msg:string) = msg.Length > len
    let ham,spam =
        dataset
        |> Array.partition (fun (docType,_) ->docType = Ham)
    let spamAndLongCount =
        spam
        |> Array.filter (fun (_,sms) -> long sms)
        |> Array.length

    let longCount =
        dataset
        |> Array.filter (fun (_,sms) -> long sms)
        |> Array.length

    let pSpam = (float spam.Length) / (float dataset.Length)

    let pLongIfSpam = float spamAndLongCount / float spam.Length

    let pLong = float longCount / float (dataset.Length)

    let pSpamIfLong = pLongIfSpam * pSpam / pLong

    pSpamIfLong

for l in 10 .. 10 .. 130 do printfn "P(Spam if Length > %i) = %.4f" l (lengthAnalysis l)

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

evaluate smartTokenizer smartTokens