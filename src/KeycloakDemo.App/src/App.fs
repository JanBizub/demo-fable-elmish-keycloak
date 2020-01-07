module App

open Fable
open Fable.Core
open Fable.React
open Fable.React.Props
open Fable.Keycloak

open Elmish
open Elmish.Navigation
open Elmish.React
open Elmish.Debug
open Fetch

open Thoth.Json

// => Authentication - KEYCLOAK ============================================================================================
//let initKeycloak()       = Fable.Core.JsInterop.importMember "./KeycloakScript.js"
//let getToken() : string  = Fable.Core.JsInterop.importMember "./KeycloakScript.js"

// => App Types ========================================================================================================
type AppModel =
    { isUiLoading: bool
      keyCloakInstance: Keycloak
      nameFromAPI: string }
    static member Empty =
        let kcConfig = JsInterop.createEmpty<KeycloakConfig>
        kcConfig.url <- Some "http://localhost:8080/auth"
        kcConfig.realm <- "demo"
        kcConfig.clientId <- "fable-react-client"

        { isUiLoading = false
          keyCloakInstance = kcConfig |> Keycloak.create
          nameFromAPI = "No car loaded from API, please request one!" }

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

    let loadCarNames token =
        let request() =
            promise {
                let! r = fetch apiUrl [ requestHeaders [ (Authorization(sprintf "Bearer %s" token)) ] ]
                let! t = r.text()
                return Decode.Auto.unsafeFromString<string> t }
        Cmd.OfPromise.either request () ReceiveNameFromAPI UiError


// => App State ========================================================================================================
let update msg (model: AppModel) =
    match msg with
    | InitKeycloak ->
        let keyCloakInitOptions = JsInterop.createEmpty<KeycloakInitOptions>
        keyCloakInitOptions.onLoad <- Some LoginRequired
        keyCloakInitOptions.promiseType <- Some Native

        keyCloakInitOptions
        |> model.keyCloakInstance.init
        |> ignore

        model, []

    | PrintTokenToConsole ->
        match model.keyCloakInstance.idToken with
        | Some t -> printf "%s" t
        | None -> printf "Auth error: there is no auth token!"

        model, []

    | Login ->
        model, []

    | Logout ->
        model, []

    | RequestNameFromAPI ->
        let command =
            match model.keyCloakInstance.token with
            | Some token -> REST.loadCarNames token
            | None ->
                "No auth token!"
                |> exn
                |> UiError
                |> Cmd.ofMsg

        model, command

    | ReceiveNameFromAPI nameFromAPI ->
        { model with nameFromAPI = nameFromAPI }, []

    | UiError error ->
        model, []

let init _ =
    AppModel.Empty, InitKeycloak |> Cmd.ofMsg

// => View to render ===================================================================================================
let appView model dispatch =
    div []
        [ hr []
          h1 [] [ str "Keycloak Demo App" ]
          p [] [ str "Car From API:" ]
          p [] [ str model.nameFromAPI ]
          hr []
          button [ OnClick(fun _ -> PrintTokenToConsole |> dispatch) ] [ "Get Token" |> str ]
          button [ OnClick(fun _ -> RequestNameFromAPI |> dispatch) ] [ "Request name from API" |> str ]
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