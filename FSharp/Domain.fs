namespace Domain

type Observation =
    { 
        Label:string;
        Pixels:int[]
    }

type Distance = int[] * int[] -> int