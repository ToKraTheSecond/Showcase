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

type Command = Withdraw | Deposit | Exit

type Transaction =
    {
        Amount : decimal
        Operation : Command
        Timestamp : string
        WasSuccess : bool
    }