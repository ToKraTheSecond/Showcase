module Logger

open Domain
open Transactions
open System.IO

let logToConsole accountId transaction =
    printfn "Account %O: %s of %M"
        accountId (parseCommandToString transaction.Operation)
        transaction.Amount

let logToFile (account:RatedAccount) transaction =
    let fileName = account.GetField(fun a -> a.Owner.Name + ".txt")
    let filePath = Path.Combine(Path.GetTempPath(), fileName)
    File.AppendAllText(filePath, serialize transaction + "\n")

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}