module Operations

open System
open Domain

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

let auditAs operationName audit operation amount account =
    let account = operation amount account
    audit account ("activity: " + operationName + " " + amount.ToString() + "; current amount: " + account.Balance.ToString())    
    account

let getCustomerName() =
    Console.Write("Enter name (format \"FirstName LastName\"): ")
    Console.ReadLine()

let getAmount() =
    Console.Write("Enter amount (positive double with dot delimiter): ")
    Decimal.Parse(Console.ReadLine())

let getOperation() =
    Console.Write("Enter operation [deposit | withdraw | exit]: ")
    Console.ReadLine()