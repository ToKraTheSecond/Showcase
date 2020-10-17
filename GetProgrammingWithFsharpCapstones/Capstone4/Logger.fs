module Logger

open Domain
open Transactions
open System.IO

let printInitString =
    printfn "Hello!\n"
    printfn "This is banking application written in F#\n"
    printfn "You are able to:"
    printfn "* select name"
    printfn "* deposit, withdraw or exit"
    printfn "* automatically load existing account from file system"
    printfn "* deposit to overdrawn account, but withdrawal from overdrawn acount is not allowed\n"

let logToConsole accountId transaction =
    printfn "Account %O: %s of %M"
        accountId
        (parseCommandToString transaction.Operation)
        transaction.Amount

let logCurrentAccountAmountToConsole (ratedAccount:RatedAccount) =
    printfn "Current balance: %M" (ratedAccount.GetField(fun a -> a.Balance))

let logToFile (account:RatedAccount) transaction =
    let fileName = account.GetField(fun a -> a.Owner.Name + ".txt")
    let filePath = Path.Combine(Path.GetTempPath(), fileName)
    File.AppendAllText(filePath, serialize transaction + "\n")

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}