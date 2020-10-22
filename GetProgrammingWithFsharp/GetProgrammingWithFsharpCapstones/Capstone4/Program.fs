open Domain
open Logger
open Operations

[<EntryPoint>]
let main argv =
    printInitString
    
    let account = loadAccount { Name = getCustomerName() }

    readConsoleCommand
    |> Seq.choose isCommandValid
    |> Seq.takeWhile ((<>) Exit)
    |> Seq.choose tryGetBankOperation
    |> Seq.map getAmount
    |> Seq.fold processCommand account
    |> ignore

    0