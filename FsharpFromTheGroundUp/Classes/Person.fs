namespace Classes

open System

type Person(name : string, favColor : string) =

    do
        if String.IsNullOrWhiteSpace(name) then
            raise <| ArgumentException("Null or empty", "message")

    let favoriteColor =
        if String.IsNullOrWhiteSpace(favColor) then
            "(None)"
        else
            favColor

    member this.Description() =
        sprintf "Name: %s, favorite color: %s" name favoriteColor
