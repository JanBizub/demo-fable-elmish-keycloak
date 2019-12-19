// MIT License
//
// Copyright 2017 Brett Epps <https://github.com/eppsilon>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
  /// URL to the Keycloak server, for example: http://keycloak-server/auth
  url: string option
  /// Name of the realm, for example: 'myrealm'
  realm: string
  /// Client identifier, example: 'myapp'
  clientId: string
  }

type KeycloakInitOptions = {
  /// Adds a [cryptographic nonce](https://en.wikipedia.org/wiki/Cryptographic_nonce)
  /// to verify that the authentication response matches the request.
  /// @default true
  useNonce: bool option
  
  /// Allows to use different adapter:
  /// - {string} default - using browser api for redirects
  /// - {string} cordova - using cordova plugins 
  /// - {function} - allows to provide custom function as adapter.
  adapter: KeycloakAdapterName option
  
  /// Specifies an action to do on load.
  onLoad: KeycloakOnLoad option
  
  /// Set an initial value for the token.
  token: string option
  
  /// Set an initial value for the refresh token.
  refreshToken: string option
  
  /// Set an initial value for the id token (only together with `token` or `refreshToken`).
  idToken: string option
  
  /// Set an initial value for skew between local time and Keycloak server in
  /// seconds (only together with `token` or `refreshToken`).
  timeSkew: int option
  
  /// Set to enable/disable monitoring login state.
  /// @default true
  checkLoginIframe: bool option
  
  /// Set the interval to check login state (in seconds).
  /// @default 5
  checkLoginIframeInterval: int option
  
  /// Set the OpenID Connect response mode to send to Keycloak upon login.
  /// @default fragment After successful authentication Keycloak will redirect
  ///                   to JavaScript application with OpenID Connect parameters
  ///                   added in URL fragment. This is generally safer and
  ///                   recommended over query.
  responseMode: KeycloakResponseMode option
  
  /// Specifies a default uri to redirect to after login or logout.
  /// This is currently supported for adapter 'cordova-native' and 'default'
  redirectUri: string option
  
  /// Specifies an uri to redirect to after silent check-sso.
  /// Silent check-sso will only happen, when this redirect uri is given and
  /// the specified uri is available whithin the application.
  silentCheckSsoRedirectUri: string option
  
  /// Set the OpenID Connect flow.
  /// @default standard
  flow: KeycloakFlow option
  
  /// Set the promise type. If set to `native` all methods returning a promise
  /// will return a native JavaScript promise. If not not specified then
  /// Keycloak specific legacy promise objects will be returned instead.
  ///
  /// Since native promises have become the industry standard it is highly
  /// recommended that you always specify `native` as the promise type.
  ///
  /// Note that in upcoming versions of Keycloak the default will be changed
  /// to `native`, and support for legacy promises will eventually be removed.
  ///
  /// @default legacy
  promiseType: KeycloakPromiseType option
 
  /// Configures the Proof Key for Code Exchange (PKCE) method to use.
  /// The currently allowed method is 'S256'.
  /// If not configured, PKCE will not be used.
  pkceMethod: KeycloakPkceMethod option
  
  /// Enables logging messages from Keycloak to the console.
  /// @default false
  enableLogging: bool option
  }

type KeycloakLoginPrompt =
  | [<CompiledName "none">] None
  | [<CompiledName "login">] Login
  
type KeycloakLoginAction =
  | [<CompiledName "register">] Register

type KeycloakLoginOptions = {
  /// @private Undocumented.
  scope: string option
  
  /// Specifies the uri to redirect to after login.
  redirectUri: string option
  
  /// By default the login screen is displayed if the user is not logged into
  /// Keycloak. To only authenticate to the application if the user is already
  /// logged in and not display the login page if the user is not logged in, set
  /// this option to `'none'`. To always require re-authentication and ignore
  /// SSO, set this option to `'login'`.
  prompt: KeycloakLoginPrompt
  
  /// If value is `'register'` then user is redirected to registration page,
  /// otherwise to login page.
  action: KeycloakLoginAction

  /// Used just if user is already authenticated. Specifies maximum time since
  /// the authentication of user happened. If user is already authenticated for
  /// longer time than `'maxAge'`, the SSO is ignored and he will need to
  /// authenticate again.
  maxAge: int option
  
  /// Used to pre-fill the username/email field on the login form.
  loginHint: string option
  
  /// Used to tell Keycloak which IDP the user wants to authenticate with.
  idpHint: string option
  
  /// Specifies the desired Keycloak locale for the UI.  This differs from
  /// the locale param in that it tells the Keycloak server to set a cookie and update
  /// the user's profile to a new preferred locale.  
  kcLocale: string option
  
  /// Specifies arguments that are passed to the Cordova in-app-browser (if applicable).
  /// Options 'hidden' and 'location' are not affected by these arguments.
  /// All available options are defined at https://cordova.apache.org/docs/en/latest/reference/cordova-plugin-inappbrowser/.
  /// Example of use: { zoom: "no", hardwareback: "yes" }  
  cordovaOptions : string list
  }

type KeycloakError = {
  error             : string
  error_description : string
  }

type KeycloakProfile = {
  id: string option
  username: string option
  email: string option
  firstName: string option
  lastName: string option
  enabled: bool option
  emailVerified: bool option
  totp: bool option
  createdTimestamp: bool option  
  }

type KeycloakRoles = {
  roles : string list
  }

// todo:
type KeycloakResourceAccess = {
  //todo: 
  // what this "[key: string]: KeycloakRoles" means in Typescript? Did I translate it correctly?
  // https://www.typescriptlang.org/docs/handbook/advanced-types.html#index-types-and-index-signatures
  key: KeycloakRoles
  }

type KeycloakTokenParsed = {
  exp: int option
  iat: int option
  nonce: string option
  sub: string option
  session_state: string option
  realm_access: KeycloakRoles;
  resource_access: KeycloakResourceAccess option;
  }

[<AbstractClass>]
type Keycloak(config: KeycloakConfig) =
  /// Is true if the user is authenticated, false otherwise.
  abstract member authenticated : bool with get, set

  /// The user id.
  abstract member subject: string option
  
  /// Response mode passed in init (default value is `'fragment'`).
  abstract member responseMode: KeycloakResponseMode option

  /// Response type sent to Keycloak with login requests. This is determined
  /// based on the flow value used during initialization, but can be overridden
  /// by setting this value.
  abstract member responseType: KeycloakResponseType option

  /// Flow passed in init.
  abstract member flow: KeycloakFlow

  /// The realm roles associated with the token.
  abstract member realmAccess: KeycloakRoles

  /// The resource roles associated with the token.
  abstract member resourceAccess: KeycloakResourceAccess

  /// The base64 encoded token that can be sent in the Authorization header in
  /// requests to services.
  abstract member token : string option

  /// The parsed token as a JavaScript object.
  abstract member tokenParsed: KeycloakTokenParsed option

  /// The base64 encoded refresh token that can be used to retrieve a new token.
  abstract member refreshToken: string option

  /// The parsed refresh token as a JavaScript object.
  abstract member refreshTokenParsed: KeycloakTokenParsed option

  /// base64 encoded ID token.
  abstract member idToken: string option

  /// The parsed id token as a JavaScript object.
  abstract member idTokenParsed: KeycloakTokenParsed option

  /// The estimated time difference between the browser time and the Keycloak
  /// server in seconds. This value is just an estimation, but is accurate
  /// enough when determining if a token is expired or not.
  abstract member timeSkew: int option

  /// @private Undocumented.
  abstract member loginRequired: bool option

  /// @private Undocumented.
  abstract member authServerUrl: string option

  /// @private Undocumented.
  abstract member realm: string option

   /// @private Undocumented.
  abstract member clientId: string option

  /// @private Undocumented.
  abstract member clientSecret: string option

  /// @private Undocumented.
  abstract member redirectUri: string option

  /// @private Undocumented.
  abstract member sessionId: string option

  /// @private Undocumented.
  abstract member profile: KeycloakProfile option

  /// @private Undocumented.
  abstract member userInfo: obj option

  // todo: theese are functions. Check if they are translated correctly
  /// Called when the adapter is initialized.
  abstract member onReady: (bool -> unit) option

  /// Called when a user is successfully authenticated.
  abstract member onAuthSuccess: (unit -> unit) option

  /// Called if there was an error during authentication.
  abstract member onAuthError: (KeycloakError -> unit) option

  /// Called when the token is refreshed.
  abstract member onAuthRefreshSuccess: (unit -> unit) option

  /// Called if there was an error while trying to refresh the token.
  abstract member onAuthRefreshError: (unit -> unit) option

  /// Called if the user is logged out (will only be called if the session
  /// status iframe is enabled, or in Cordova mode).
  abstract member onAuthLogout: (unit -> unit) option 

  /// Called when the access token is expired. If a refresh token is available
  /// the token can be refreshed with Keycloak#updateToken, or in cases where
  /// it's not (ie. with implicit flow) you can redirect to login screen to
  /// obtain a new access token.
  abstract member onTokenExpired: (unit -> unit) option

  /// Called to initialize the adapter.
  /// @param initOptions Initialization options.
  /// @returns A promise to set functions to be invoked on success or error.
  abstract member init: KeycloakInitOptions -> Promise<bool>

  //todo: how to write optional parameters in F#?
  /// Redirects to login form.
  /// @param options Login options.
  abstract member login: KeycloakLoginOptions option -> Promise<unit>

  /// Redirects to logout.
  /// @param options Logout options.
  /// @param options.redirectUri Specifies the uri to redirect to after logout.
  abstract member logout: obj option -> Promise<unit>

  /// Redirects to registration form.
  /// @param options Supports same options as Keycloak#login but `action` is
  ///                set to `'register'`.
  abstract member register: obj option -> Promise<unit>

  /// Redirects to the Account Management Console.
  abstract member accountManagement: Promise<unit>

  /// Returns the URL to login form.
  /// @param options Supports same options as Keycloak#login.
  abstract member createLoginUrl: (KeycloakLoginOptions option) -> string

  /// Returns the URL to logout the user.
  /// @param options Logout options.
  /// @param options.redirectUri Specifies the uri to redirect to after logout.
  abstract member createLogoutUrl: (obj option) -> string

  /// Returns the URL to registration page.
  /// @param options Supports same options as Keycloak#createLoginUrl but
  ///                `action` is set to `'register'`.
  abstract member createRegisterUrl: (KeycloakLoginOptions option) -> string

  /// Returns the URL to the Account Management Console.
  abstract member createAccountUrl: unit -> string 

  /// Returns true if the token has less than `minValidity` seconds left before
  /// it expires.
  /// @param minValidity If not specified, `0` is used.
  abstract member isTokenExpired: int option -> bool

  /// If the token expires within `minValidity` seconds, the token is refreshed.
  /// If the session status iframe is enabled, the session status is also
  /// checked.
  /// @returns A promise to set functions that can be invoked if the token is
  ///          still valid, or if the token is no longer valid.
  /// @example
  /// ```js
  /// keycloak.updateToken(5).success(function(refreshed) {
  ///   if (refreshed) {
  ///     alert('Token was successfully refreshed');
  ///   } else {
  ///     alert('Token is still valid');
  ///   }
  /// }).error(function() {
  ///   alert('Failed to refresh the token, or the session has expired');
  /// });
  abstract member updateToken: int -> Promise<bool>

  /// Clears authentication state, including tokens. This can be useful if
  /// the application has detected the session was expired, for example if
  /// updating token fails. Invoking this results in Keycloak#onAuthLogout
  /// callback listener being invoked.
  abstract member clearToken: unit -> unit

  /// Returns true if the token has the given realm role.
  /// @param role A realm role name.
  abstract member hasRealmRole: string -> bool

  /// Returns true if the token has the given role for the resource.
  /// @param role A role name.
  /// @param resource If not specified, `clientId` is used.
  abstract member hasResourceRole: string -> string option -> bool

  /// Loads the user's profile.
  /// @returns A promise to set functions to be invoked on success or error.
  abstract member loadUserProfile: unit -> Promise<KeycloakProfile>

  /// @private Undocumented.
  abstract member loadUserInfo: unit -> Promise<obj>

// [<Import("Keycloak","keycloak-js")>]
[<RequireQualifiedAccess>]
module KeyCloak =
  [<Emit "new Keycloak($0)">]
  /// Create an instance of Keycloak
  /// @param: KeycloakConfig Configuration
  let create (config: KeycloakConfig): Keycloak = jsNative