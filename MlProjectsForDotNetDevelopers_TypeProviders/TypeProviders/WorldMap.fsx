#I @"C:\Users\krata\.nuget\packages\" // TODO reference it from solution folder
#r @"fsharp.data\3.3.3\lib\net45\FSharp.Data.dll"
#r @"deedle\2.3.0\lib\net45\Deedle.dll"
#r @"r.net\1.7.0\lib\net40\RDotNet.dll"
#r @"rprovider\1.1.22\lib\net40\RProvider.Runtime.dll"
#r @"rprovider\1.1.22\lib\net40\RProvider.dll"
#r @"deedle.rplugin\2.3.0\lib\net451\Deedle.RProvider.Plugin.dll"

open FSharp.Data
open Deedle
open RProvider
open RProvider.``base``
open Deedle.RPlugin

(*
Install if not present
open RProvider.utils
R.install_packages(["rworldmap"])
*)
open RProvider.rworldmap

let wb = WorldBankData.GetDataContext()
let countries = wb.Countries

let population2000 = series [ for c in countries -> c.Code, c.Indicators.``Population, total``.[2000]]
let population2010 = series [ for c in countries -> c.Code, c.Indicators.``Population, total``.[2010]]
let surface = series [ for c in countries -> c.Code, c.Indicators.``Surface area (sq. km)``.[2010]]

let dataframe = 
    frame [ 
        "Pop2000", population2000
        "Pop2010", population2010
        "Surface", surface ]
dataframe?Code <- dataframe.RowKeys

// creating a map with rworldmap
let map = R.joinCountryData2Map(dataframe,"ISO3","Code")
R.mapCountryData(map,"Pop2000")

dataframe?Density <- dataframe?Pop2010 / dataframe?Surface
let map2 = R.joinCountryData2Map(dataframe,"ISO3","Code")
R.mapCountryData(map2,"Density")

dataframe?Growth  <- (dataframe?Pop2010 - dataframe?Pop2000) / dataframe?Pop2000
let map3 = R.joinCountryData2Map(dataframe,"ISO3","Code")
R.mapCountryData(map3,"Growth")
