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
  | PrintTokenToConsole
  | RequestNameFromAPI
  | ReceiveNameFromAPI of string
  | UiError of exn

module REST =
  let apiUrl = "http://localhost:51776/car"
  
  //let loadCarNames () =
  //  let request () = promise {
  //    let! r = fetch apiUrl [requestHeaders [(Authorization (sprintf "Bearer %s" (getToken()) ))]] 
  //    let! t = r.text()
  //    return Decode.Auto.unsafeFromString<string> t
  //  }
  //  Cmd.OfPromise.either request () ReceiveNameFromAPI RestError

let keyCloakInitOptions = {
  useNonce                  = None
  adapter                   = None
  onLoad                    = Some LoginRequired
  token                     = None
  refreshToken              = None
  idToken                   = None
  timeSkew                  = None
  checkLoginIframe          = None
  checkLoginIframeInterval  = None
  responseMode              = None
  redirectUri               = None
  silentCheckSsoRedirectUri = None
  flow                      = None
  promiseType               = Some Native
  pkceMethod                = None
  enableLogging             = None
}

// => App State ========================================================================================================
let update msg (model: AppModel) =
  match msg with
  | InitKeycloak ->
    model.keyCloakInstance.init (keyCloakInitOptions) |> ignore

    model, []

  | PrintTokenToConsole ->
    match model.keyCloakInstance.idToken with
    | Some t -> printf "%s" t
    | None   -> printf "Auth error: there is no auth token!"

    model, []

  | Login ->  
    model, []

  | Logout ->
    model, []

  | RequestNameFromAPI ->
    model, []
   
  | ReceiveNameFromAPI nameFromAPI ->
    model, []

  | UiError error ->
    model, []
  
let init _ = 
 AppModel.Empty, InitKeycloak |> Cmd.ofMsg

// => View to render ===================================================================================================
let appView model dispatch =
  div [] [
    hr []
    h1 [] [str "Keycloak Demo App"]
    hr []
    button [OnClick ( fun _ -> PrintTokenToConsole |> dispatch)] ["Get Token" |> str]
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