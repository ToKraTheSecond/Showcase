// My solution
[0..999]                                         
|> Seq.filter (fun x -> x % 3 = 0 || x % 5 = 0)  
|> Seq.sum

// Other cool solutions
// http://www.fssnip.net/1a/title/Project-Euler-1
[0..999]                                         
|> Seq.filter (fun x -> x % 3 = 0 || x % 5 = 0)  
|> Seq.fold (+) 0
// |> Seq.reduce (+)

let condition x = x % 3 = 0 || x % 5 = 0
[0..999] |> Seq.filter condition |> Seq.fold (+) 0

let rec multiple x =
  if x = 1000 then 0
  elif x%3 = 0 || x%5 = 0 then x + multiple (x+1)
  else multiple (x+1)
  
multiple 1