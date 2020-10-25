// requires absolute path
// not sure how to use relative path
#r @"C:\Users\krata\git\FsharpShowcase\FsharpRefCsharp\FsharpRefCsharp\CSharpProject\obj\Debug\netstandard2.0\CSharpProject.dll"

open CSharpProject

let simon = Person "Simon"
simon.PrintName()

let longhand =
    [ "Tony"; "Fred"; "Samantha"; "Brad"; "Sophie"]
    |> List.map(fun name -> Person(name)) // calling a constructor explicitly

let shorthand =
    [ "Tony"; "Fred"; "Samantha"; "Brad"; "Sophie"]
    |> List.map Person // treating a constructor like a standard function

(*
open System.Collections.Generic

type PersonComparer() =
    interface IComparer<Person> with
        member this.Compare(x, y) = x.Name.CompareTo(y.Name)

let pComparer = PersonComparer() :> IComparer<Person> // explicitly upcast ":>"
pComparer.Compare(simon, Person "Fred")
*)

// Create instance of an interface without creating an intermediary type
(*
open System.Collections.Generic

let pComparer =
    {
        new IComparer<Person> with // interface definition
            member this.Compare(x, y) = x.Name.CompareTo(y.Name) // interface implementation
    }
*)

// Option combinators for classes and nullable types

(*
open System

let blank:string = null
let name = "Vera"
let number = Nullable 10
let blankAsOption = blank |> Option.ofObj // null maps to none
let nameAsOption = name |> Option.ofObj // non-null maps to Some
let numberAsOption = number |> Option.ofNullable
let unsafeName = Some "Fred" |> Option.toObj // options can be mapped back to classes or nullable types
*)