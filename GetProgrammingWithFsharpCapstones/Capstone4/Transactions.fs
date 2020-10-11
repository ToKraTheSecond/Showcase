module Transactions

open System
open Domain

let parseCommandToString command =
    match command with
    | Withdraw -> "withdraw"
    | Deposit -> "deposit"

let parseStringToCommand string =
    match string with
    | "withdraw" -> Withdraw
    | "deposit" -> Deposit

let serialize transaction =
    sprintf "%M***%s***%s***%b"
        transaction.Amount
        (parseCommandToString transaction.Operation)
        transaction.Timestamp
        transaction.WasSuccess

let deserialize (serializedString:string) =
    let splitted = serializedString.Split([| "***" |], StringSplitOptions.None)
    { 
        Amount = Decimal.Parse(splitted.[0]);
        Operation = parseStringToCommand splitted.[1];
        Timestamp = splitted.[2];
        WasSuccess = Boolean.Parse(splitted.[3]);
    }