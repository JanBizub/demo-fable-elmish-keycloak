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
let aad_clientID  = "<CLIENT ID>"
let aad_authority = "https://login.microsoftonline.com/<TENANT ID>"
let aad_replyUrl  = "https://localhost:1010/"

// => App Types ========================================================================================================
type AppModel = 
  { isUiLoading : bool
    nameFromAPI : string }
  static member Empty =
    { isUiLoading = false
      nameFromAPI = "" }

type Msg =
  | Login
  | Logout

// => App State ========================================================================================================
let update msg model =
  match msg with
  | Login ->  
    model, []

  | Logout ->
    model, []
  
let init _ = 
 AppModel.Empty, []

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