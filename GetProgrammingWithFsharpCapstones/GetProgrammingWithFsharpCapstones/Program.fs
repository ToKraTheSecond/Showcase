open System
open Domain
open Operations
open Auditing

(*
what this should do
1) allow a customer to deposit and withdraw from 
   an account and maintain a running total of the
   balance
2) decline transcaction if the customer tries to
   withdraw more money than is in the account
   ( balance stay as is )
3) system writes out all transactions to a data
   store (is pluggable - filesystem, console, .. )
4) do not couple code to filesystem, console input
6) app executable as a console application
7) at start ask for the customer's name and opening
   balance -> creates account in memory
8) app repeatedly asks whether the customer wants
   to deposit or withdraw money from the account
9) app prints out updated balance to the user
   after every transaction.

Don't worry about
- opening multiple accounts
- overdraw warning - just keep the same balance

F# stuff that will be used:
- records, tuples, functions, high-order functions
*)
let getCustomerName() =
    Console.Write("Enter name in format \"FirstName LastName\": ")
    Console.ReadLine()

let getAmount() =
    Console.Write("Enter amount as positive number: ")
    Decimal.Parse(Console.ReadLine())

let getOperation() =
    Console.Write("Enter operation [deposit | withdraw | exit]: ")
    Console.ReadLine()

let withdrawWithConsoleAudit = auditAs "withdraw" consoleAudit withdraw
let depositWithConsoleAudit = auditAs "deposit" consoleAudit deposit

[<EntryPoint>]
let main argv =
    let name = getCustomerName()
    let balance = getAmount()

    let cus = { Name = name }
    let mutable acc = { AccountId = Guid.NewGuid(); Owner = cus; Balance = balance }

    while true do
        let operation = getOperation()
        if operation = "exit" then Environment.Exit 0
        let balance = getAmount()
        acc <-
            match operation with
            | "deposit" ->  depositWithConsoleAudit balance acc
            | "withdraw" -> withdrawWithConsoleAudit balance acc
            | _ -> acc
        ()
    0
