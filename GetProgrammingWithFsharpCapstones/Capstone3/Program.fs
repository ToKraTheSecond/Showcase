open System
open Domain
open Operations

[<EntryPoint>]
let main argv =
    let openingAccount = { AccountId = Guid.NewGuid(); Owner = { Name = getCustomerName() }; Balance = 0M }
    Console.Write ("\n Current balance is " + openingAccount.Balance.ToString())

    consoleCommands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmountConsole
    |> Seq.fold processCommand openingAccount
    |> ignore

    0
