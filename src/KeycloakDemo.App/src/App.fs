module App
open Elmish
open Elmish.Navigation
open Elmish.React
open Elmish.Debug
open Fable
open Fable.React
open Fable.React.Props
open Thoth.Json
open Fetch
open Fable.Keycloak
open Fable.Core

// => Authentication - KEYCLOAK ============================================================================================
//let initKeycloak()       = Fable.Core.JsInterop.importMember "./KeycloakScript.js"
//let getToken() : string  = Fable.Core.JsInterop.importMember "./KeycloakScript.js"

// => App Types ========================================================================================================
type AppModel = 
  { 
  isUiLoading       : bool
  keyCloakInstance  : Keycloak
  }
  static member Empty = { 
    isUiLoading      = false
    keyCloakInstance = KeyCloak.create ({
      url      = Some "http://localhost:8080/auth"
      realm    = "demo"
      clientId = "fable-react-client"
    })
  }

type Msg =
  | InitKeycloak
  | Login
  | Logout
  | RequestNameFromAPI
  | ReceiveNameFromAPI of string
  | RestError of exn

module REST =
  let apiUrl = "http://localhost:51776/car"
  
  //let loadCarNames () =
  //  let request () = promise {
  //    let! r = fetch apiUrl [requestHeaders [(Authorization (sprintf "Bearer %s" (getToken()) ))]] 
  //    let! t = r.text()
  //    return Decode.Auto.unsafeFromString<string> t
  //  }
  //  Cmd.OfPromise.either request () ReceiveNameFromAPI RestError

// => App State ========================================================================================================
let update msg (model: AppModel) =
  match msg with
  | InitKeycloak ->
    model.keyCloakInstance.init ({
      onLoad      = Some LoginRequired
      promiseType = Some Native
    }) |> ignore

    model, []

  | Login ->  
    model, []

  | Logout ->
    model, []

  | RequestNameFromAPI ->
    let token = model.keyCloakInstance.token
    printfn "%s" token
    
    model, []
   
  | ReceiveNameFromAPI nameFromAPI ->
    model, []

  | RestError error ->
    model, []
  
let init _ = 
 AppModel.Empty, InitKeycloak |> Cmd.ofMsg

// => View to render ===================================================================================================
let appView model dispatch =
  div [] [
    hr []
    h1 [] [str "Keycloak Demo App"]
    hr []
    button [OnClick ( fun _ -> RequestNameFromAPI |> dispatch)] ["Get Token" |> str]
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