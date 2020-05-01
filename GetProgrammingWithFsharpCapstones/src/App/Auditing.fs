module Auditing
open Domain
open System
open System.IO

let fileSystemAudit account message =
    let fileName =  account.Owner.Name + ".txt"
    let filePath = Path.Combine(Path.GetTempPath(), fileName)
    File.AppendAllText(filePath, message + "\n")


let consoleAudit account message =
    Console.WriteLine("Account " + account.AccountId.ToString() + ": " + message)