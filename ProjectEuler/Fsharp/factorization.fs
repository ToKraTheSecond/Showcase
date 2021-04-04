namespace ProjectEuler

module Factorization =

    let rec factorize n possibleFactor factorSet =
        if possibleFactor = n then
            possibleFactor::factorSet
        elif n % possibleFactor = 0L then
            factorize (n / possibleFactor) possibleFactor (possibleFactor::factorSet)
        else
            factorize n (possibleFactor + 1L) factorSet