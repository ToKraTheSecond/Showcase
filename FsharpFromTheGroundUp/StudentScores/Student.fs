namespace StudentScores

type Student =
    {
        Name : string
        Id : string
        MeanScore : float
        MinScore : float
        MaxScore : float
    }

 module Student =

    let fromString (s : string) =
        let elements = s.Split('\t')
        let name = elements.[0]
        let id = elements.[1]
        let scores =
            elements
            |> Array.skip 2
            |> Array.map (Float.fromStringOr 50.0)
        let meanScore = scores |> Array.average
        let minScore = scores |> Array.min
        let maxScore = scores |> Array.max
        {
            Name = name
            Id = id
            MeanScore = meanScore
            MinScore = minScore
            MaxScore = maxScore
        }

    let printSummary (student : Student) =
        printfn "%s\t%s\t%0.1f\t%0.1f\t%0.1f" student.Name student.Id student.MeanScore student.MinScore student.MaxScore
