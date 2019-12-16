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
let initKeycloak() = Fable.Core.JsInterop.importMember "./KeycloakScript.js"

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
  
let init _ = 
 AppModel.Empty, InitKeycloak |> Cmd.ofMsg

// => View to render ===================================================================================================
let appView model dispatch =
  div [] [
    hr []
    h1 [] [str "Keycloak Demo App"]
    hr []
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