module Auditing
open Domain
open System
open System.IO

let fileSystemAudit account message =
    let fileName =  account.AccountId.ToString() + ".txt"
    let folderpath = sprintf "C:\\temp\\learnfs\\capstone2\\%s" account.Owner.Name
    let filePath = Path.Combine(folderpath, fileName)
    let createDirectory = Directory.CreateDirectory(folderpath)
    let writeToFile = File.AppendAllText(filePath, message + "\n") // lol first line is empty
    ()

let consoleAudit account message =
    Console.WriteLine("Account " + account.AccountId.ToString() + ": " + message)
    ()