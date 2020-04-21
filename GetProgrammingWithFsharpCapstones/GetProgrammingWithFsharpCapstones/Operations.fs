module Operations
open Domain

let deposit amount account =
    {
        AccountId = account.AccountId;
        Owner = account.Owner;
        Balance = account.Balance + amount
    }

let withdraw amount account =
    if amount > account.Balance then
        account
    else
        {
            AccountId = account.AccountId;
            Owner = account.Owner;
            Balance = account.Balance - amount
        }

let auditAs operationName audit operation amount account =
    let account = operation amount account
    audit account ("activity: " + operationName + " " + amount.ToString() + "; current amount: " + account.Balance.ToString())    
    account