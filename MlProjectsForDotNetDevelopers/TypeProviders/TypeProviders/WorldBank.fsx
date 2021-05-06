#I @"C:\Users\krata\.nuget\packages\" // TODO reference it from solution folder
#r @"fsharp.data\3.3.3\lib\net45\FSharp.Data.dll"
#r @"r.net\1.7.0\lib\net40\RDotNet.dll"
#r @"rprovider\1.1.22\lib\net40\RProvider.Runtime.dll"
#r @"rprovider\1.1.22\lib\net40\RProvider.dll"
#r @"deedle\2.3.0\lib\net45\Deedle.dll"

open FSharp.Data
open RProvider
open RProvider.``base``
open RProvider.graphics
open Deedle

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

// Creating and plotting an R data frame
let pollution = [ for c in countries -> c.Indicators.``CO2 emissions (kt)``.[2000]]
let education = [ for c in countries -> c.Indicators.``School enrollment, secondary (% gross)``.[2000]]

let rdf =
    [
        "Pop2000", box pop2000
        "Pop2010", box pop2010
        "Surface", box surface
        "Pollution", box pollution
        "Education", box education ]
    |> namedParams
    |> R.data_frame

// Scatterplot of all features
rdf |> R.plot
rdf |> R.summary |> R.print

// Creating series and data frames with Deedle
let series1 = series [ "Alpha", 1.; "Bravo", 2.; "Delta", 4. ]
let series2 = series [ "Bravo", 20.; "Charlie", 30.; "Delta", 40. ]
let toyFrame = frame [ "First", series1; "Second", series2 ]

series1 |> Stats.sum
toyFrame |> Stats.mean
toyFrame?Second |> Stats.mean

toyFrame?New <- toyFrame?First + toyFrame?Second
toyFrame |> Stats.mean