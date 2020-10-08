module Operations

open System
open System.IO
open Domain
open Transactions
open Logger

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

let isCommandValid command =
    (Set.ofList [ 'd'; 'w'; 'x']).Contains(command)

let isCommandStop command =
    command.ToString().Equals("x")

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

let processTransaction operationName operation amount account =
    let updatedAccount = operation amount account

    let transaction =
        match (account = updatedAccount) with
        | true -> { Amount = amount; Operation = operationName; Timestamp = DateTime.UtcNow.ToString(); WasSuccess = false }
        | false -> { Amount = amount; Operation = operationName; Timestamp = DateTime.UtcNow.ToString(); WasSuccess = true }

    logToFile account transaction
    logToConsole account.AccountId transaction
    updatedAccount

let rec getAmount command =
    printf "Enter amount: "
    let amount = Console.ReadLine()
    match isAmountValid amount with
    | true ->
        match command with
        | 'd' -> ('d', Decimal.Parse(amount))
        | 'w' -> ('w', Decimal.Parse(amount))
        | 'x' -> ('x', 0M)
    | false -> getAmount command

let readConsoleCommand = seq {
    while true do
        printf "(d)eposit, (w)ithdraw or e(x)it: "
        let char = Console.ReadKey().KeyChar
        printfn ""
        yield char }

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> processTransaction "deposit" deposit amount account
    | 'w' -> processTransaction "withdraw" withdraw amount account
    | 'x' -> account

let getCommandAmountTuple transaction =
    match transaction.Operation with
    | "deposit" -> ('d', transaction.Amount)
    | "withdraw" -> ('w', transaction.Amount)

let loadAccount customer =
    let initAccount = {
        AccountId = Guid.NewGuid();
        Owner = customer;
        Balance = 0M
    }

    let fileName = customer.Name  + ".txt"
    let filePath = Path.Combine(Path.GetTempPath(), fileName)

    let transactions =
        match File.Exists(filePath) with
        | true -> readLines filePath |> Seq.map deserialize
        | false -> Seq.empty<Domain.Transaction>

    match Seq.length transactions = 0 with
    | true -> printfn "No previous transactions found. New account created."
    | false -> printfn "Previous transactions found. Loaded."

    transactions
    |> Seq.sortBy(fun transaction -> transaction.Timestamp)
    |> Seq.fold(fun account  transaction ->
        if transaction.Operation =  "withdraw" then account |> withdraw transaction.Amount
        else account |> deposit transaction.Amount) initAccount