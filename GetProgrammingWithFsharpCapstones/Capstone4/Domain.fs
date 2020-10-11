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