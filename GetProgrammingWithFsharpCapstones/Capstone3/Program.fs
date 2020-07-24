open System
open Domain
open Operations

[<EntryPoint>]
let main argv =
    let account = loadAccount { Name = getCustomerName() }

    Console.Write ("\n Current balance is " + account.Balance.ToString())

    consoleCommands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmountConsole
    |> Seq.fold processCommand account
    |> ignore

    0
