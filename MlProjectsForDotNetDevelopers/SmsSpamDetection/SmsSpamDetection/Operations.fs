module Operations

open System.Text.RegularExpressions

open Domain
open NaiveBayes.Classifier

let proportion count total = float count / float total

let laplace count total = float (count+1) / float (total+1)

let countIn (group:TokenizedDoc seq) (token:Token) =
    group
    |> Seq.filter (Set.contains token)
    |> Seq.length

let analyze (group:TokenizedDoc seq) (totalDocs:int) (classificationTokens:Token Set) =
    let groupSize = group |> Seq.length
    let score token =
        let count = countIn group token
        laplace count groupSize
    let scoredTokens =
        classificationTokens
        |> Set.map (fun token -> token, score token)
        |> Map.ofSeq
    let groupProportion = proportion groupSize totalDocs

    {
        Proportion = groupProportion
        TokenFrequencies = scoredTokens
    }

let learn (docs:(_ * string) []) (tokenizer:Tokenizer) (classificationTokens:Token Set) =
    let total = docs.Length
    docs
    |> Array.map (fun (label,docs) -> label,tokenizer docs)
    |> Seq.groupBy fst
    |> Seq.map (fun (label,group) -> label,group |> Seq.map snd)
    |> Seq.map (fun (label,group) -> label,analyze group total classificationTokens)
    |> Seq.toArray

let train (docs:(_ * string) []) (tokenizer:Tokenizer) (classificationTokens:Token Set) =
    let groups = learn docs tokenizer classificationTokens
    let classifier = classify groups tokenizer
    classifier

let lengthAnalysis len (dataset:(_ * string) []) =
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