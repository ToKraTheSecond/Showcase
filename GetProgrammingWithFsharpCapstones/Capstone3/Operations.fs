module Operations

open System
open Domain

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account =
    if amount > account.Balance then 
        Console.WriteLine("Transaction rejected!")
        account
    else { account with Balance = account.Balance - amount }

let auditAs operationName audit operation amount account =
    let updatedAccount = operation amount account
    audit account (" balance: " + updatedAccount.Balance.ToString())
    updatedAccount

let getCustomerName() =
    Console.WriteLine("Enter name: ")
    Console.ReadLine()

let getAmount() =
    Console.WriteLine("Enter amount (positive double with dot delimiter): ")
    Decimal.Parse(Console.ReadLine())

let getOperation() =
    Console.WriteLine("Choose operation [deposit | withdraw | exit]: ")
    Console.ReadLine()