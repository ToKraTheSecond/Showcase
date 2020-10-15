﻿module Operations

open System
open System.IO
open Domain
open Transactions
open Logger

let tryParseCommand inputChar =
    match inputChar with
    | 'd' -> Some (AccountCommand Deposit)
    | 'w' -> Some (AccountCommand Withdraw)
    | 'x' -> Some Exit
    | _ -> None

let tryGetBankOperation cmd =
    match cmd with
    | Exit -> None
    | AccountCommand op -> Some op

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

let isCommandValid commandChar =
    tryParseCommand commandChar

let isNameValid name =
    match Seq.length name with
    | 0 -> false
    | _ -> name |> Seq.forall (fun c -> System.Char.IsLetter(c))

let isAmountValid amount =
    match Seq.length amount with
    | 0 -> false
    | _ -> amount |> Seq.forall (fun d -> System.Char.IsDigit(d))

let rec getCustomerName () =
    printf "Enter name: "
    let name = Console.ReadLine()
    match isNameValid name with
    | true -> name
    | false -> getCustomerName()

let processTransaction (command:BankOperation) operation amount account =
    let updatedAccount = operation amount account

    let transaction =
        match (account = updatedAccount) with
        | true -> { Amount = amount; Operation = command; Timestamp = DateTime.UtcNow.ToString(); WasSuccess = false }
        | false -> { Amount = amount; Operation = command; Timestamp = DateTime.UtcNow.ToString(); WasSuccess = true }

    logToFile account transaction
    logToConsole account.AccountId transaction
    updatedAccount

// Book prefers non recursive method witch returns option type
let rec getAmount (command:BankOperation) =
    printf "Enter amount: "
    let amount = Console.ReadLine()
    match isAmountValid amount with
    | true ->
        match command with
        | Deposit -> (Deposit, Decimal.Parse(amount))
        | Withdraw -> (Withdraw, Decimal.Parse(amount))
    | false -> getAmount command

let readConsoleCommand = seq {
    while true do
        printf "(d)eposit, (w)ithdraw or e(x)it: "
        let char = Console.ReadKey().KeyChar
        printfn ""
        yield char }

let processCommand (account:Account) (command:BankOperation, amount:decimal) =
    match command with
    | Deposit -> processTransaction Deposit deposit amount account
    | Withdraw -> processTransaction Withdraw withdraw amount account

let getCommandAmountTuple transaction =
    match transaction.Operation with
    | Deposit -> (Deposit, transaction.Amount)
    | Withdraw -> (Withdraw, transaction.Amount)

let tryFindAccountFolder ownerName =
    let fileName = ownerName  + ".txt"
    let filePath = Path.Combine(Path.GetTempPath(), fileName)
    match File.Exists(filePath) with
    | true -> Some filePath
    | false -> None

let loadAccount customer =
    let initAccount = {
        AccountId = Guid.NewGuid();
        Owner = customer;
        Balance = 0M
    }

    let transactions =
        match (tryFindAccountFolder customer.Name) with
        | Some filePath -> readLines filePath |> Seq.map deserialize
        | None -> Seq.empty<Domain.Transaction>

    match Seq.length transactions = 0 with
    | true -> printfn "No previous transactions found. New account created."
    | false -> printfn "Previous transactions found. Loaded."

    transactions
    |> Seq.sortBy(fun transaction -> transaction.Timestamp)
    |> Seq.fold(fun account  transaction ->
        if transaction.Operation =  Withdraw then account |> withdraw transaction.Amount
        else account |> deposit transaction.Amount) initAccount