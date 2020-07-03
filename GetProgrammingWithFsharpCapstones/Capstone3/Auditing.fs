module Auditing
open Domain
open System
open System.IO

let serialized transaction =
    sprintf "%M***%s***%s***%b"
        transaction.Amount
        transaction.Operation
        transaction.Timestamp
        transaction.WasSuccess

let fileSystemAudit account transaction =
    let fileName =  account.Owner.Name + ".txt"
    let filePath = Path.Combine(Path.GetTempPath(), fileName)
    File.AppendAllText(filePath, serialized transaction + "\n")

let consoleAudit account transaction =
    Console.WriteLine(
        "\n Account  ID: "
        + account.AccountId.ToString()
        + " "
        + serialized transaction)