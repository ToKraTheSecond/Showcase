module Transactions

open System
open Domain

let serialized transaction =
    sprintf "%M***%s***%s***%b"
        transaction.Amount
        transaction.Operation
        transaction.Timestamp
        transaction.WasSuccess

let deserialize (serializedString:string) =
    let splitted = serializedString.Split([| "***" |], StringSplitOptions.None)
    { 
        Amount = Decimal.Parse(splitted.[0]);
        Operation = splitted.[1];
        Timestamp = splitted.[2];
        WasSuccess = Boolean.Parse(splitted.[3]);
    }