open System

[<EntryPoint>]
let main argv =

    let isEven x =
        x % 2 = 0

    let todayIsThursday() = // function without input params needs brackets
        DateTime.Now.DayOfWeek = DayOfWeek.Thursday

    // Array comprehension
    let numbers =
        [|
            if todayIsThursday() then 42
            for i in 1..9 do
                let x = i * i
                if x |> isEven then
                    x // implicit yield 
                    // explicit yield may be present in older code
            999
        |]

    let summedSquares =
        Array.init 1000 (fun i ->
            let x = i + 1
            x * x)
        // [| for i in 1..1000 -> i * i |]
        // for i in 1..1000 do i * i
        // for i in 1..1000 -> yield i * i
        |> Array.sum

    printfn "%A" numbers
    printfn "%i" summedSquares
    
    0