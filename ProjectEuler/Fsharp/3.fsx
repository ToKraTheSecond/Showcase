#load "Factorization.fs"

open ProjectEuler.Factorization

factorize 2L [] 600851475143L |> Seq.max

// Other cool solutions
// https://github.com/JustinPealing/euler-fs/blob/master/problem03.fsx

let rec nextFactor (x:bigint) (n:bigint) = 
    if x % n = 0I then n else nextFactor x (n + 1I)

let rec factors (x:bigint) =
    let f = nextFactor x 2I
    let otherFactors = if f = x then [] else factors (x / f)
    f::otherFactors

printfn "%A" (factors 600851475143I)

// https://codereview.stackexchange.com/a/69060
let primeFactors = 
    let rec recPrimeFactors primes i = function
        | n when 2L*i > n -> n::primes
        | n -> match n % i with
               | 0L -> recPrimeFactors (i::primes) 2L (n / i)
               | _ -> recPrimeFactors primes (i + 1L) n
    recPrimeFactors [] 2L

600851475143L |> primeFactors |> List.head |> printfn "%d";;