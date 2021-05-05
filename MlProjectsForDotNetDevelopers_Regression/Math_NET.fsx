#r "nuget: FSharp.Data"
#r "nuget: XPlot.Plotly"
#r "nuget: MathNet.Numerics.FSharp"

open FSharp.Data
open XPlot.Plotly
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

let A = vector [ 1.; 2.; 3. ]
let B = matrix [
            [ 1.; 2. ]
            [ 3.; 4. ]
            [ 5.; 6. ] 
        ]

let C = A * A
let D = A * B
let E = A * B.Column(1)

[<Literal>]
let dataPath = __SOURCE_DIRECTORY__ + "/Data/day.csv"
type Data = CsvProvider<dataPath>
let dataset = Data.Load(dataPath)
let data = dataset.Rows

type Vec = Vector<float>
type Mat = Matrix<float>

type Obs = Data.Row
type Model = Obs -> float

let update alpha (theta0, theta1) (obs:Obs) =
    let y = float obs.Cnt
    let x = float obs.Instant
    let theta0' = theta0 - 2. * alpha * 1. * (theta0 + theta1 * x - y)
    let theta1' = theta1 - 2. * alpha * x * (theta0 + theta1 * x - y)
    theta0', theta1'

let cost (theta:Vec) (Y:Vec) (X:Mat) =
    let ps = Y - (theta * X.Transpose())
    ps * ps |> sqrt

let predict (theta:Vec) (v:Vec) = theta * v

let X = matrix [ for obs in data -> [ 1.; float obs.Instant ]]
let Y = vector [ for obs in data -> float obs.Cnt ]

let theta = vector [6000.; -4.5]

predict theta (X.Row(0))
cost theta Y X

let estimate (Y:Vec) (X:Mat) = (X.Transpose() * X).Inverse() * X.Transpose() * Y