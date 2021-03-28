namespace StudentScores

type TestResult =
    | Absent
    | Excused
    | Voided
    | Scored of float

module TestResult =

    let fromString s =
        match s with
        | "A" -> Absent
        | "E" -> Excused
        | "V" -> Voided
        | _ -> 
            Scored (s |> float)

    let tryEffectiveScore (testResult : TestResult) = 
        match testResult with
        | Absent -> Some 0.0
        | Excused
        | Voided -> None
        | Scored score -> Some score