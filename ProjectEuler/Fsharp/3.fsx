// inspiration on how to work with numbers bigger than 32-bit integer from:
// https://jeremybytes.blogspot.com/2016/07/getting-prime-factors-in-f-with-good.html
#load "Factorization.fs"

open ProjectEuler.Factorization

factorize 600851475143L 2L [] |> Seq.max