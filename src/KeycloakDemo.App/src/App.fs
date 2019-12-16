module App
open Elmish
open Elmish.Navigation
open Elmish.React
open Elmish.Debug
open Fable.React
open Fable.React.Props
open Thoth.Json
open Fetch

// => Authentication - KEYCLOAK ============================================================================================
let initKeycloak()       = Fable.Core.JsInterop.importMember "./KeycloakScript.js"
let getToken() : string  = Fable.Core.JsInterop.importMember "./KeycloakScript.js"

// => App Types ========================================================================================================
type AppModel = 
  { isUiLoading : bool
    nameFromAPI : string }
  static member Empty =
    { isUiLoading = false
      nameFromAPI = "" }

type Msg =
  | InitKeycloak
  | Login
  | Logout
  | RequestNameFromAPI
  | ReceiveNameFromAPI of string
  | RestError of exn

// http://localhost:5000/car

module REST =
  let apiUrl = "http://localhost:51776/car"
  
  let loadCarNames () =
    let request () = promise {
      let! r = fetch apiUrl [requestHeaders [(Authorization (sprintf "Bearer %s" (getToken()) ))]] 
      let! t = r.text()
      return Decode.Auto.unsafeFromString<string> t
    }
    Cmd.OfPromise.either request () ReceiveNameFromAPI RestError

// => App State ========================================================================================================
let update msg model =
  match msg with
  | InitKeycloak ->
    initKeycloak()
    model, []

  | Login ->  
    model, []

  | Logout ->
    model, []

  | RequestNameFromAPI ->
    model, REST.loadCarNames ()
   
  | ReceiveNameFromAPI nameFromAPI ->
    {model with nameFromAPI = nameFromAPI}, []

  | RestError error ->
    model, []
  
let init _ = 
 AppModel.Empty, InitKeycloak |> Cmd.ofMsg

// => View to render ===================================================================================================
let appView model dispatch =
  div [] [
    hr []
    h1 [] [str "Keycloak Demo App"]
    p  [] [(sprintf "name from API is: %s" model.nameFromAPI) |> str]
    hr []
    button [OnClick ( fun _ -> printf "token: %s" (getToken()) )] ["Get Token" |> str]
    button [OnClick ( fun _ -> RequestNameFromAPI |> dispatch)] ["Get Name from API" |> str]
    hr [] ]

Program.mkProgram init update appView
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "keycloak-demo-app"
//#if DEBUG
|> Program.withDebugger
//#endif
|> Program.run