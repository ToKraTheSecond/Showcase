module Operations

open System
open Domain

let deposit amount account =
    let newAcc = { account with Balance = account.Balance + amount }
    Console.Write ("\n Current balance is " + newAcc.Balance.ToString())
    newAcc

let withdraw amount account =
    if amount > account.Balance then 
        Console.Write "\n Transaction rejected!"
        Console.Write ("\n Current balance is " + account.Balance.ToString())
        account
    else 
    let newAcc = { account with Balance = account.Balance - amount }
    Console.Write ("\n Current balance is " + newAcc.Balance.ToString())
    newAcc

let auditAs operationName audit operation amount account =
    let updatedAccount = operation amount account
    audit account (" balance: " + updatedAccount.Balance.ToString())
    updatedAccount

let getCustomerName() =
    Console.Write "\n Enter name: "
    Console.ReadLine()

let isValidCommand command =
    let validCommands = Set.ofList [ 'd'; 'w'; 'x']
    validCommands.Contains(command)

let isStopCommand command =
    command.ToString().Equals('x')

let getAmountConsole command =
    Console.Write "\n Enter amount: "
    let amount = Decimal.Parse(Console.ReadLine())
    match command with
    | 'd' -> ('d', amount)
    | 'w' -> ('w', amount)
    | 'x' -> ('x', 0M)

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> deposit amount account
    | 'w' -> withdraw amount account
    | 'x' -> account

let consoleCommands = seq {
    while true do
        Console.Write "\n (d)eposit, (w)ithdraw or e(x)it: "
        yield Console.ReadKey().KeyChar }