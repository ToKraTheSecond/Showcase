#load "Domain.fs"
#load "Operations.fs"

open System
open Domain
open Operations

let isValidCommand command =
    let validCommands = Set.ofList [ 'd'; 'w'; 'e']
    validCommands.Contains(command)

let isStopCommand command =
    command.ToString().Equals('e')

let getAmount command =
    match command with
    | 'd' -> ('d', 50M)
    | 'w' -> ('w', 25M)
    | 'e' -> ('e', 0M)

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> deposit amount account
    | 'w' -> withdraw amount account
    | 'e' -> account

let openingAccount = { AccountId = Guid.Empty; Owner = { Name = "Isaac"}; Balance = 0M }
let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w']

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount
