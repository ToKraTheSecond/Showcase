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

type Transaction =
    {
        Amount : decimal
        Operation : string
        Timestamp : string
        WasSuccess : bool
    }