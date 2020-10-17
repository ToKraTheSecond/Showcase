namespace Domain

type Customer =
    {
        Name : string
    }

type Account =
    {
        AccountId : System.Guid
        Owner : Customer
        Balance : decimal
    }

type BankOperation = Deposit | Withdraw
type Command = AccountCommand of BankOperation | Exit

type Transaction =
    {
        Amount : decimal
        Operation : BankOperation
        Timestamp : string
        WasSuccess : bool
    }

type CreditAccount = CreditAccount of Account
type RatedAccount =
    | InCredit of CreditAccount
    | Overdrawn of Account
    member this.GetField getter =
        match this with
        | InCredit (CreditAccount account) -> getter account
        | Overdrawn account -> getter account