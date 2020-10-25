open Domain
open Operations

[<EntryPoint>]
let main argv =
    let account = loadAccount { Name = getCustomerName() }

    readConsoleCommand
    |> Seq.filter isCommandValid
    |> Seq.takeWhile (not << isCommandStop)
    |> Seq.map getAmount
    |> Seq.fold processCommand account
    |> ignore

    0