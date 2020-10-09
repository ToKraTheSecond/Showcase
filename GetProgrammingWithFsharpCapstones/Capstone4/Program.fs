open Domain
open Operations

[<EntryPoint>]
let main argv =
    let account = loadAccount { Name = getCustomerName() }

    readConsoleCommand
    |> Seq.choose isCommandValid
    |> Seq.takeWhile ((<>) Exit)
    |> Seq.map getAmount
    |> Seq.fold processCommand account
    |> ignore

    0