#load "Domain.fs"
#load "Operations.fs"

open System
open Domain
open Operations

let dummyTransactions =
    [
        {Amount = 25M; Operation = "deposit"; Timestamp = "moc"; WasSuccess = true};
        {Amount = 5M; Operation = "withdraw"; Timestamp = "moc"; WasSuccess = true};
        {Amount = 25M; Operation = "withdraw"; Timestamp = "moc"; WasSuccess = true};
    ]