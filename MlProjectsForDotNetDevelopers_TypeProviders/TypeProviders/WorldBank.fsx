#I @"C:\Users\krata\.nuget\packages\" // TODO reference it from solution folder
#r @"fsharp.data\3.3.3\lib\net45\FSharp.Data.dll"
#r @"r.net\1.7.0\lib\net40\RDotNet.dll"
#r @"rprovider\1.1.22\lib\net40\RProvider.Runtime.dll"
#r @"rprovider\1.1.22\lib\net40\RProvider.dll"

open FSharp.Data
open RProvider
open RProvider.``base``
open RProvider.graphics

let wb = WorldBankData.GetDataContext()
let countries = wb.Countries

let pop2000 = [ for c in countries -> c.Indicators.``Population, total``.[2000]]
let pop2010 = [ for c in countries -> c.Indicators.``Population, total``.[2010]]

// Retrive an (F#) list of country surfaces
let surface = [ for c in countries -> c.Indicators.``Surface area (sq. km)``.[2010]]
// Produce summary statistics
R.summary(surface) |> R.print
R.hist(surface)
R.hist(surface |> R.log)
R.plot(surface, pop2010)