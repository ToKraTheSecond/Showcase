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