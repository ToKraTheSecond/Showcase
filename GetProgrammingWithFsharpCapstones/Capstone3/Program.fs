open System
open Domain
open Operations
open Auditing

let withdrawWithConsoleAudit = auditAs "withdraw" fileSystemAudit withdraw
let depositWithConsoleAudit = auditAs "deposit" fileSystemAudit deposit

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> depositWithConsoleAudit amount account
    | 'w' -> withdrawWithConsoleAudit amount account
    | 'x' -> account

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
