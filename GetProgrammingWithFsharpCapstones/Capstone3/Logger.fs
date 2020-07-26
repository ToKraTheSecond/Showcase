module Logger
open Domain
open System.IO

let logToConsole accountId transaction =
    printfn "Account %O: %s of %M (approved: %b)"
        accountId transaction.Operation
        transaction.Amount
        transaction.WasSuccess

let serialized transaction =
    sprintf "%M***%s***%s***%b"
        transaction.Amount
        transaction.Operation
        transaction.Timestamp
        transaction.WasSuccess

let logToFile account transaction =
    let fileName =  account.Owner.Name + ".txt"
    let filePath = Path.Combine(Path.GetTempPath(), fileName)
    File.AppendAllText(filePath, serialized transaction + "\n")