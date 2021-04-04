// inspiration on how to work with numbers bigger than 32-bit integer from:
// https://jeremybytes.blogspot.com/2016/07/getting-prime-factors-in-f-with-good.html

let rec factorize n possibleFactor factorSet =
    if possibleFactor = n then
        possibleFactor::factorSet
    elif n % possibleFactor = 0L then
        factorize (n / possibleFactor) possibleFactor (possibleFactor::factorSet)
    else
        factorize n (possibleFactor + 1L) factorSet