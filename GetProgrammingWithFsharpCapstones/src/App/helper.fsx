#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open System
open Domain
open Operations
open Auditing

// Dummy test cus1 and acc1

let cus1 = { Name = "Isaac" }
let acc1 = { AccountId = Guid.NewGuid(); Owner = cus1; Balance = 100M }

// Test out withdraw

let newAccount = acc1 |> withdraw 10M
newAccount.Balance = 80M // should be true

// Test out console auditor

consoleAudit acc1 "Testing console audit"

// Composite magic
acc1 |> deposit 100M |> withdraw 50M

let withdrawWithConsoleAudit = auditAs "withdraw" consoleAudit withdraw
let depositWithConsoleAudit = auditAs "deposit" consoleAudit deposit

acc1
|> depositWithConsoleAudit 100M
|> withdrawWithConsoleAudit 50M