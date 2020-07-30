open Domain
open Operations

[<EntryPoint>]
let main argv =
    let account = loadAccount { Name = getCustomerName() }

    consoleCommands
    |> Seq.filter isCommandValid
    |> Seq.takeWhile (not << isCommandStop)
    |> Seq.map getAmountConsole
    |> Seq.fold processCommand account
    |> ignore

    0