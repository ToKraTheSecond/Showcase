#I @"C:\Users\krata\.nuget\packages\" // TODO reference it from solution folder
#r @"fsharp.data\3.3.3\lib\net45\FSharp.Data.dll"

open FSharp.Data

type Questions = JsonProvider<"""https://api.stackexchange.com/2.2/questions?site=stackoverflow""">

let csQuestions = """https://api.stackexchange.com/2.2/questions?site=stackoverflow&tagged=C%23"""

Questions.Load(csQuestions).Items |> Seq.iter (fun q -> printfn "%s" q.Title)

[<Literal>]
let sample = """{"items":[
{"tags":["java","arrays"],"owner":{"reputation":1,"user_id":4852416,"user_type":"registered","profile_image":"https://lh6.googleusercontent.com/-YnW8ioov-DU/AAAAAAAAAAI/AAAAAAAAABk/trncNZE44cw/photo.jpg?sz=128","display_name":"Izzwan iwan","link":"http://stackoverflow.com/users/4852416/izzwan-iwan"},"is_answered":true,"view_count":17,"answer_count":2,"score":-6,"last_activity_date":1430429499,"creation_date":1430429108,"last_edit_date":1430429190,"question_id":29978017,"link":"http://stackoverflow.com/questions/29978017/i-have-ti-try-to-compile-this-code-but-there-some-errors-that-i-cant-find","title":"I have ti try to compile this code but there some errors that I can&#39;t find"},
{"tags":["javascript","jquery","html"],"owner":{"reputation":1,"user_id":4713230,"user_type":"registered","profile_image":"https://www.gravatar.com/avatar/3ae146c365e92dd1187317862de38a56?s=128&d=identicon&r=PG&f=1","display_name":"eolallerup","link":"http://stackoverflow.com/users/4713230/eolallerup"},"is_answered":false,"view_count":12,"answer_count":2,"score":-1,"last_activity_date":1430429496,"creation_date":1430429219,"question_id":29978048,"link":"http://stackoverflow.com/questions/29978048/how-do-i-change-the-src-of-image-tags-with-a-certain-class","title":"How do I change the src of image tags with a certain class?"},
{"tags":["php","magento","roles"],"owner":{"reputation":1,"user_id":4545317,"user_type":"registered","profile_image":"https://lh4.googleusercontent.com/-vHB4PHEkBQg/AAAAAAAAAAI/AAAAAAAAABs/k1Ls8eAuHV4/photo.jpg?sz=128","display_name":"Sachin S","link":"http://stackoverflow.com/users/4545317/sachin-s"},"is_answered":false,"view_count":3,"answer_count":0,"score":0,"last_activity_date":1430429412,"creation_date":1430375446,"last_edit_date":1430429412,"question_id":29960562,"link":"http://stackoverflow.com/questions/29960562/fetching-admin-details-from-role-id-in-magento","title":"Fetching admin details from role id in magento"},
{"tags":["javascript","jquery","html","twitter-bootstrap"],"owner":{"reputation":1,"user_id":3488220,"user_type":"registered","profile_image":"https://www.gravatar.com/avatar/1b5847d8779ecb4b9eadb82585659109?s=128&d=identicon&r=PG&f=1","display_name":"learner9100","link":"http://stackoverflow.com/users/3488220/learner9100"},"is_answered":false,"view_count":25,"answer_count":1,"score":0,"last_activity_date":1430429411,"creation_date":1430336482,"last_edit_date":1430382317,"question_id":29953054,"link":"http://stackoverflow.com/questions/29953054/places-search-box-inside-bootstrap-active-tab-content-is-not-displaying","title":"Places search box inside bootstrap active tab content is not displaying"}],
"has_more":true,"quota_max":300,"quota_remaining":299}"""

type HardCodedQuestions = JsonProvider<sample>

[<Literal>]
let javaQuery = "https://api.stackexchange.com/2.2/questions?site=stackoverflow&tagged=java"

let javaQuestions = HardCodedQuestions.Load(javaQuery).Items |> Seq.iter (fun q -> printfn "%s" q.Title)