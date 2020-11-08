namespace Domain

type DocType =
    | Ham
    | Spam

type Token = string
type Tokenizer = string -> Token Set
type TokenizedDoc = Token Set

type DocsGroup =
    {
        Proportion:float;
        TokenFrequencies:Map<Token,float>;
    }