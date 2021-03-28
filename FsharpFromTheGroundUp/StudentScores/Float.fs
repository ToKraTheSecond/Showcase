namespace StudentScores

module Float =

    let tryFromString s =
        match s with
        | "N/A" -> None
        | _ -> Some (float s)

    let fromStringOr d s =
        s
        |> tryFromString
        |> Option.defaultValue d