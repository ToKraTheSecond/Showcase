module Operations

open System
open Domain
open Logger
open System.IO

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

let auditAs operationName operation amount account =
    let updatedAccount = operation amount account

    let transaction =
        match (account = updatedAccount) with
        | true -> { Amount = amount; Operation = operationName; Timestamp = DateTime.UtcNow.ToString(); WasSuccess = false }
        | false -> { Amount = amount; Operation = operationName; Timestamp = DateTime.UtcNow.ToString(); WasSuccess = true }

    logToFile account transaction
    logToConsole account.AccountId transaction
    updatedAccount

let isNameValid name =
    match Seq.length name with
    | 0 -> false
    | _ -> name |> Seq.forall (fun c -> System.Char.IsLetter(c))

let rec getCustomerName () =
    printfn "Enter name: "
    let name = Console.ReadLine()
    match isNameValid name with
    | true -> name
    | false -> getCustomerName()

let isCommandValid command =
    let validCommands = Set.ofList [ 'd'; 'w'; 'x']
    validCommands.Contains(command)

let isCommandStop command =
    command.ToString().Equals("x")

let getAmountConsole command =
    Console.Write "\n Enter amount: "
    let amount = Decimal.Parse(Console.ReadLine())
    match command with
    | 'd' -> ('d', amount)
    | 'w' -> ('w', amount)
    | 'x' -> ('x', 0M)

let consoleCommands = seq {
    while true do
        Console.Write "\n (d)eposit, (w)ithdraw or e(x)it: "
        yield Console.ReadKey().KeyChar }

let processCommand (account:Account) (command:char, amount:decimal) =
    match command with
    | 'd' -> auditAs "deposit" deposit amount account
    | 'w' -> auditAs "withdraw" withdraw amount account
    | 'x' -> account

let getCommandAmountTuple transaction =
    match transaction.Operation with
    | "deposit" -> ('d', transaction.Amount)
    | "withdraw" -> ('w', transaction.Amount)

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let deserialize (serializedString:string) =
    let splitted = serializedString.Split([| "***" |], StringSplitOptions.None)
    { 
        Amount = Decimal.Parse(splitted.[0]);
        Operation = splitted.[1];
        Timestamp = splitted.[2];
        WasSuccess = Boolean.Parse(splitted.[3]);
    }

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
        | false -> Seq.empty<Transaction>

    match Seq.length transactions = 0 with
    | true -> printfn "No previous transactions found. New account created."
    | false -> printfn "Previous transactions found. Loaded."

    transactions
    |> Seq.sortBy(fun transaction -> transaction.Timestamp)
    |> Seq.fold(fun account  transaction ->
        if transaction.Operation =  "withdraw" then account |> withdraw transaction.Amount
        else account |> deposit transaction.Amount) initAccount