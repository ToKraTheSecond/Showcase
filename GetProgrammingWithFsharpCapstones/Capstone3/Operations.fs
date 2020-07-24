module Operations

open System
open Domain
open Logger
open System.IO

let deposit amount account =
    let newAcc = { account with Balance = account.Balance + amount }
    Console.Write ("\n Current balance after deposit of: " + amount.ToString() + " is " + newAcc.Balance.ToString())
    newAcc

let withdraw amount account =
    if amount > account.Balance then 
        Console.Write ("\n Withdraw of " + amount.ToString() + " rejected! Current balance is " + account.Balance.ToString())
        account
    else 
    let newAcc = { account with Balance = account.Balance - amount }
    Console.Write ("\n Current balance after withdraw of: " + amount.ToString() + " is " + newAcc.Balance.ToString())
    newAcc

let auditAs operationName audit operation amount account =
    let updatedAccount = operation amount account
    let transaction = {
        Amount = amount;
        Operation = operationName
        Timestamp = DateTime.UtcNow.ToString()
        WasSuccess = true } // TODO: How to get this state
    audit account transaction
    updatedAccount

let getCustomerName() =
    Console.Write "\n Enter name: "
    Console.ReadLine()

let isValidCommand command =
    let validCommands = Set.ofList [ 'd'; 'w'; 'x']
    validCommands.Contains(command)

let isStopCommand command =
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
    | 'd' ->  auditAs "deposit" logToFile deposit amount account
    | 'w' -> auditAs "withdraw" logToFile withdraw amount account
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

    match Seq.length transactions with
    | 0 -> Console.Write "\n No previous transactions found! \n Empty account created. "
    | _ -> Console.Write "\n Previous transactions: "

    let account =
        transactions
        |> Seq.sortBy(fun transaction -> transaction.Timestamp)
        |> Seq.map getCommandAmountTuple
        |> Seq.fold processCommand initAccount

    Console.Write ("\n Current balance is " + account.Balance.ToString())

    account

