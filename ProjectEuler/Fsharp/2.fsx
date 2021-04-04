// My solution
// Seq.unfold [function [currentState]] [initialState]
Seq.unfold (fun (a, b) -> Some(a + b, (b, a + b))) (0, 1)
|> Seq.takeWhile (fun x -> x <= 3999999 )
|> Seq.filter (fun x -> x % 2 = 0)
|> Seq.sum

// Other cool solutions
// https://stackoverflow.com/a/45776903
let rec fib a b =
    let current = a + b
    match current with
    | _ when current >= 3999999 -> []
    | _ -> current :: fib b current 

fib 0 1
|> Seq.filter (fun x -> x % 2 = 0)
|> Seq.sum