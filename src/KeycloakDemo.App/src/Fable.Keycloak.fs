module rec Fable.Keycloak
open Fable.Core
open Fable.Core.JS
  
[<StringEnum>]
type KeycloakAdapterName = 
  | [<CompiledName "cordova">] Cordova
  | [<CompiledName "cordova-native">] CordovaName
  | [<CompiledName "default">] Default
  // todo | Any? 

[<StringEnum>]
type KeycloakOnLoad =
  | [<CompiledName "login-required">] LoginRequired
  | [<CompiledName "check-sso">] CheckSSO

[<StringEnum>]
type KeycloakResponseMode = 
  | [<CompiledName "query">] Query
  | [<CompiledName "fragment">] Fragment

[<StringEnum>]
type KeycloakResponseType = 
  | [<CompiledName "code">] Code
  | [<CompiledName "id_token token">] IdTokenToken
  | [<CompiledName "code id_token token">] CodeIdTokenToken

[<StringEnum>]
type KeycloakFlow =
  | [<CompiledName "standard">] Standard
  | [<CompiledName "implicit">] Implicit
  | [<CompiledName "hybrid">] Hybrid
  
[<StringEnum>]
type KeycloakPromiseType =
  | [<CompiledName "legacy">] Legacy
  | [<CompiledName "native">] Native

[<StringEnum>]
type KeycloakPkceMethod =
  | [<CompiledName "S256">] S256

type KeycloakConfig = {
  url      : string option
  realm    : string
  clientId : string
}

type KeycloakInitOptions = {
  onLoad      : KeycloakOnLoad option
  promiseType : KeycloakPromiseType option
}
  
type KeycloakError = {
  error             : string
  error_description : string
  }

[<AbstractClass>]
type Keycloak(config: KeycloakConfig) =
  abstract member authenticated: bool with get, set
  abstract member token: string with get, set
  abstract member refreshToken: string with get, set

  abstract member init: KeycloakInitOptions -> Promise<bool>

// [<Import("Keycloak","keycloak-js")>]

[<Emit "new Keycloak({
        url: 'http://localhost:8080/auth',
        realm: 'demo',
        clientId: 'fable-react-client'
    })">]
let createKeyCloak () : Keycloak = jsNative





















































//type KeycloakInitOptions = {
//  useNonce                  : bool option
//  adapter                   : KeycloakAdapterName option
//  token                     : string option
//  refreshToken              : string option
//  idToken                   : string option
//  timeSkew                  : int option
//  checkLoginIframe          : bool option
//  checkLoginIframeInterval  : int option
//  responseMode              : KeycloakResponseMode option
//  redirectUri               : string option
//  silentCheckSsoRedirectUri : string option
//  flow                      : KeycloakFlow option
//  promiseType               : KeycloakPromiseType option
//  pkceMethod                : KeycloakPkceMethod option
//  enableLogging             : bool option
//  }

//type [<AllowNullLiteral>] KeycloakConfig =
//  abstract url        : string option with get, set
//  abstract realm      : string with get, set
//  abstract clientId   : string with get, set

//module KeycloakConfig = 
//  let create realm clientId = { url = None; realm = realm; clientId = clientId }
//  let withUrl url config = { config with url = Some url }

//type [<AllowNullLiteral>] KeycloakInitOptions = 
//  abstract useNonce                  : bool option with get, set
//  abstract adapter                   : KeycloakAdapterName option with get, set
//  abstract token                     : string option with get, set
//  abstract refreshToken              : string option with get, set
//  abstract idToken                   : string option with get, set
//  abstract timeSkew                  : int option with get, set
//  abstract checkLoginIframe          : bool option with get, set
//  abstract checkLoginIframeInterval  : int option with get, set
//  abstract responseMode              : KeycloakResponseMode option with get, set
//  abstract redirectUri               : string option with get, set
//  abstract silentCheckSsoRedirectUri : string option with get, set
//  abstract flow                      : KeycloakFlow option with get, set
//  abstract promiseType               : KeycloakPromiseType option with get, set
//  abstract pkceMethod                : KeycloakPkceMethod option with get, set
//  abstract enableLogging             : bool option with get, set
  
//type [<AllowNullLiteral>] KeycloakError =
//  abstract error             : string with get, set
//  abstract error_description : bool option with get, set