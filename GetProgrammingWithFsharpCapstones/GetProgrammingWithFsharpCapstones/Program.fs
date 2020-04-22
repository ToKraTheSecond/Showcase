open System
open Domain
open Operations
open Auditing

// Learn more about F# at https://fsharp.org
// See the 'F# Tutorial' project for more help.

let getCustomerName() =
    Console.Write("Enter name: ")
    Console.ReadLine()

let getAmount() =
    Console.Write("Enter amount: ")
    Decimal.Parse(Console.ReadLine())

let getOperation() =
    Console.Write("Enter operation: ")
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
