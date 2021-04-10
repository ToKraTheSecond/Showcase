open System
open Classes

[<EntryPoint>]
let main argv =
    let namePrompt = ConsolePrompt("Please enter your name", 3)
    namePrompt.ColorScheme <- ConsoleColor.Cyan, ConsoleColor.DarkBlue
    let name = namePrompt.GetValue()
    printfn "Hello %s" name

    let colorPrompt = ConsolePrompt("What is your favorite color (press Enter if you dont have one)", 1)
    let favColor = colorPrompt.GetValue()

    let person = Person(name, favColor)
    printfn "%s" (person.Description())

    0