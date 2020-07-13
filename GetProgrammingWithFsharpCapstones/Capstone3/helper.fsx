#load "Domain.fs"
#load "Logger.fs"
#load "Operations.fs"

open System
open Domain
open Operations

let dummyTransactions =
    [        
        {Amount = 5M; Operation = "withdraw"; Timestamp = "7/11/2020 6:57:31"; WasSuccess = true};
        {Amount = 25M; Operation = "deposit"; Timestamp = "7/11/2020 6:57:28"; WasSuccess = true};
        {Amount = 25M; Operation = "withdraw"; Timestamp = "7/11/2020 6:57:34"; WasSuccess = true};
    ]

let dummyAccountId = System.Guid.Empty;