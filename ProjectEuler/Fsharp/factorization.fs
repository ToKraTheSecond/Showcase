namespace ProjectEuler

module Factorization =

    // inspiration on how to work with numbers bigger than 32-bit integer from:
    // https://jeremybytes.blogspot.com/2016/07/getting-prime-factors-in-f-with-good.html
    let rec factorize possibleFactor factorSet = function
        | n when n = possibleFactor -> possibleFactor::factorSet
        | n -> match n % possibleFactor with
            | 0L -> factorize possibleFactor (possibleFactor::factorSet) (n / possibleFactor)
            | _ -> factorize (possibleFactor + 1L) factorSet n