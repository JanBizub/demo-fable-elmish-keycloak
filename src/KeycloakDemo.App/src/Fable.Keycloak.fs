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

[<StringEnum>]
type KeycloakLoginPrompt =
  | [<CompiledName "none">] Nothing
  | [<CompiledName "login">] Login
  
[<StringEnum>]
type KeycloakLoginAction =
  | [<CompiledName "register">] Register

type [<AllowNullLiteral>] KeycloakConfig =
  /// URL to the Keycloak server, for example: http://keycloak-server/auth
  abstract url: string option with get, set
  /// Name of the realm, for example: 'myrealm'
  abstract realm: string with get, set
  /// Client identifier, example: 'myapp'
  abstract clientId: string with get, set

type [<AllowNullLiteral>] KeycloakInitOptions =
  /// Adds a [cryptographic nonce](https://en.wikipedia.org/wiki/Cryptographic_nonce)
  /// to verify that the authentication response matches the request.
  abstract useNonce: bool option with get, set
  /// Allows to use different adapter:
  /// 
  /// - {string} default - using browser api for redirects
  /// - {string} cordova - using cordova plugins 
  /// - {function} - allows to provide custom function as adapter.
  abstract adapter: KeycloakAdapterName option with get, set
  /// Specifies an action to do on load.
  abstract onLoad: KeycloakOnLoad option with get, set
  /// Set an initial value for the token.
  abstract token: string option with get, set
  /// Set an initial value for the refresh token.
  abstract refreshToken: string option with get, set
  /// Set an initial value for the id token (only together with `token` or
  /// `refreshToken`).
  abstract idToken: string option with get, set
  /// Set an initial value for skew between local time and Keycloak server in
  /// seconds (only together with `token` or `refreshToken`).
  abstract timeSkew: float option with get, set
  /// Set to enable/disable monitoring login state.
  abstract checkLoginIframe: bool option with get, set
  /// Set the interval to check login state (in seconds).
  abstract checkLoginIframeInterval: float option with get, set
  /// Set the OpenID Connect response mode to send to Keycloak upon login.
  abstract responseMode: KeycloakResponseMode option with get, set
  /// Specifies a default uri to redirect to after login or logout.
  /// This is currently supported for adapter 'cordova-native' and 'default'
  abstract redirectUri: string option with get, set
  /// Specifies an uri to redirect to after silent check-sso.
  /// Silent check-sso will only happen, when this redirect uri is given and
  /// the specified uri is available whithin the application.
  abstract silentCheckSsoRedirectUri: string option with get, set
  /// Set the OpenID Connect flow.
  abstract flow: KeycloakFlow option with get, set
  /// Set the promise type. If set to `native` all methods returning a promise
  /// will return a native JavaScript promise. If not not specified then
  /// Keycloak specific legacy promise objects will be returned instead.
  /// 
  /// Since native promises have become the industry standard it is highly
  /// recommended that you always specify `native` as the promise type.
  /// 
  /// Note that in upcoming versions of Keycloak the default will be changed
  /// to `native`, and support for legacy promises will eventually be removed.
  abstract promiseType: KeycloakPromiseType option with get, set
  /// Configures the Proof Key for Code Exchange (PKCE) method to use.
  /// The currently allowed method is 'S256'.
  /// If not configured, PKCE will not be used.
  abstract pkceMethod: KeycloakPkceMethod option with get, set
  /// Enables logging messages from Keycloak to the console.
  abstract enableLogging: bool option with get, set
 
type [<AllowNullLiteral>] KeycloakLoginOptions =
  abstract scope: string option with get, set
  /// Specifies the uri to redirect to after login.
  abstract redirectUri: string option with get, set
  /// By default the login screen is displayed if the user is not logged into
  /// Keycloak. To only authenticate to the application if the user is already
  /// logged in and not display the login page if the user is not logged in, set
  /// this option to `'none'`. To always require re-authentication and ignore
  /// SSO, set this option to `'login'`.
  abstract prompt: U2<string, string> option with get, set
  /// If value is `'register'` then user is redirected to registration page,
  /// otherwise to login page.
  abstract action: string option with get, set
  /// Used just if user is already authenticated. Specifies maximum time since
  /// the authentication of user happened. If user is already authenticated for
  /// longer time than `'maxAge'`, the SSO is ignored and he will need to
  /// authenticate again.
  abstract maxAge: float option with get, set
  /// Used to pre-fill the username/email field on the login form.
  abstract loginHint: string option with get, set
  /// Used to tell Keycloak which IDP the user wants to authenticate with.
  abstract idpHint: string option with get, set
  /// Sets the 'ui_locales' query param in compliance with section 3.1.2.1
  ///  of the OIDC 1.0 specification.
  abstract locale: string option with get, set
  /// Specifies the desired Keycloak locale for the UI.  This differs from
  ///  the locale param in that it tells the Keycloak server to set a cookie and update
  ///  the user's profile to a new preferred locale.
  abstract kcLocale: string option with get, set
  //todo: 
  // /**
  //  * Specifies arguments that are passed to the Cordova in-app-browser (if applicable).
  //  * Options 'hidden' and 'location' are not affected by these arguments.
  //  * All available options are defined at https://cordova.apache.org/docs/en/latest/reference/cordova-plugin-inappbrowser/.
  //  * Example of use: { zoom: "no", hardwareback: "yes" }
  //  */
  // cordovaOptions?: { [optionName: string]: string };

type [<AllowNullLiteral>] KeycloakError =
  abstract error: string with get, set
  abstract error_description: string with get, set

type [<AllowNullLiteral>] KeycloakProfile =
  abstract id: string option with get, set
  abstract username: string option with get, set
  abstract email: string option with get, set
  abstract firstName: string option with get, set
  abstract lastName: string option with get, set
  abstract enabled: bool option with get, set
  abstract emailVerified: bool option with get, set
  abstract totp: bool option with get, set
  abstract createdTimestamp: float option with get, set

type [<AllowNullLiteral>] KeycloakRoles =
  abstract roles: ResizeArray<string> with get, set

type [<AllowNullLiteral>] KeycloakResourceAccess =
  [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> KeycloakRoles with get, set

type [<AllowNullLiteral>] KeycloakTokenParsed =
  abstract exp: float option with get, set
  abstract iat: float option with get, set
  abstract nonce: string option with get, set
  abstract sub: string option with get, set
  abstract session_state: string option with get, set
  abstract realm_access: KeycloakRoles option with get, set
  abstract resource_access: KeycloakResourceAccess option with get, set

/// A client for the Keycloak authentication server.
/// @see {@link https://keycloak.gitbooks.io/securing-client-applications-guide/content/topics/oidc/javascript-adapter.html|Keycloak JS adapter documentation}
[<AbstractClass>]
type Keycloak(config: KeycloakConfig) =
 /// Is true if the user is authenticated, false otherwise.
  abstract authenticated: bool option with get, set
  /// The user id.
  abstract subject: string option with get, set
  /// Response mode passed in init (default value is `'fragment'`).
  abstract responseMode: KeycloakResponseMode option with get, set
  /// Response type sent to Keycloak with login requests. This is determined
  /// based on the flow value used during initialization, but can be overridden
  /// by setting this value.
  abstract responseType: KeycloakResponseType option with get, set
  /// Flow passed in init.
  abstract flow: KeycloakFlow option with get, set
  /// The realm roles associated with the token.
  abstract realmAccess: KeycloakRoles option with get, set
  /// The resource roles associated with the token.
  abstract resourceAccess: KeycloakResourceAccess option with get, set
  /// The base64 encoded token that can be sent in the Authorization header in
  /// requests to services.
  abstract token: string option with get, set
  /// The parsed token as a JavaScript object.
  abstract tokenParsed: KeycloakTokenParsed option with get, set
  /// The base64 encoded refresh token that can be used to retrieve a new token.
  abstract refreshToken: string option with get, set
  /// The parsed refresh token as a JavaScript object.
  abstract refreshTokenParsed: KeycloakTokenParsed option with get, set
  /// The base64 encoded ID token.
  abstract idToken: string option with get, set
  /// The parsed id token as a JavaScript object.
  abstract idTokenParsed: KeycloakTokenParsed option with get, set
  /// The estimated time difference between the browser time and the Keycloak
  /// server in seconds. This value is just an estimation, but is accurate
  /// enough when determining if a token is expired or not.
  abstract timeSkew: float option with get, set
  abstract loginRequired: bool option with get, set
  abstract authServerUrl: string option with get, set
  abstract realm: string option with get, set
  abstract clientId: string option with get, set
  abstract clientSecret: string option with get, set
  abstract redirectUri: string option with get, set
  abstract sessionId: string option with get, set
  abstract profile: KeycloakProfile option with get, set
  //todo:abstract userInfo: TypeLiteral_01 option with get, set
  /// Called when the adapter is initialized.
  abstract onReady: ?authenticated: bool -> unit
  /// Called when a user is successfully authenticated.
  abstract onAuthSuccess: unit -> unit
  /// Called if there was an error during authentication.
  abstract onAuthError: errorData: KeycloakError -> unit
  /// Called when the token is refreshed.
  abstract onAuthRefreshSuccess: unit -> unit
  /// Called if there was an error while trying to refresh the token.
  abstract onAuthRefreshError: unit -> unit
  /// Called if the user is logged out (will only be called if the session
  /// status iframe is enabled, or in Cordova mode).
  abstract onAuthLogout: unit -> unit
  /// Called when the access token is expired. If a refresh token is available
  /// the token can be refreshed with Keycloak#updateToken, or in cases where
  /// it's not (ie. with implicit flow) you can redirect to login screen to
  /// obtain a new access token.
  abstract onTokenExpired: unit -> unit
  /// <summary>Called to initialize the adapter.</summary>
  /// <param name="initOptions">Initialization options.</param>
  abstract init: initOptions: KeycloakInitOptions -> Promise<bool>
  /// <summary>Redirects to login form.</summary>
  /// <param name="options">Login options.</param>
  abstract login: ?options: KeycloakLoginOptions -> Promise<unit>
  /// <summary>Redirects to logout.</summary>
  /// <param name="options">Logout options.</param>
  abstract logout: ?options: obj -> Promise<unit>
  /// <summary>Redirects to registration form.</summary>
  /// <param name="options">Supports same options as Keycloak#login but `action` is
  /// set to `'register'`.</param>
  abstract register: ?options: obj -> Promise<unit>
  /// Redirects to the Account Management Console.
  abstract accountManagement: unit -> Promise<unit>
  /// <summary>Returns the URL to login form.</summary>
  /// <param name="options">Supports same options as Keycloak#login.</param>
  abstract createLoginUrl: ?options: KeycloakLoginOptions -> string
  /// <summary>Returns the URL to logout the user.</summary>
  /// <param name="options">Logout options.</param>
  abstract createLogoutUrl: ?options: obj -> string
  /// <summary>Returns the URL to registration page.</summary>
  /// <param name="options">Supports same options as Keycloak#createLoginUrl but
  /// `action` is set to `'register'`.</param>
  abstract createRegisterUrl: ?options: KeycloakLoginOptions -> string
  /// Returns the URL to the Account Management Console.
  abstract createAccountUrl: unit -> string
  /// <summary>Returns true if the token has less than `minValidity` seconds left before
  /// it expires.</summary>
  /// <param name="minValidity">If not specified, `0` is used.</param>
  abstract isTokenExpired: ?minValidity: float -> bool
  /// If the token expires within `minValidity` seconds, the token is refreshed.
  /// If the session status iframe is enabled, the session status is also
  /// checked.
  abstract updateToken: minValidity: float -> Promise<bool>
  /// Clears authentication state, including tokens. This can be useful if
  /// the application has detected the session was expired, for example if
  /// updating token fails. Invoking this results in Keycloak#onAuthLogout
  /// callback listener being invoked.
  abstract clearToken: unit -> unit
  /// <summary>Returns true if the token has the given realm role.</summary>
  /// <param name="role">A realm role name.</param>
  abstract hasRealmRole: role: string -> bool
  /// <summary>Returns true if the token has the given role for the resource.</summary>
  /// <param name="role">A role name.</param>
  /// <param name="resource">If not specified, `clientId` is used.</param>
  abstract hasResourceRole: role: string * ?resource: string -> bool
  /// Loads the user's profile.
  abstract loadUserProfile: unit -> Promise<KeycloakProfile>
  abstract loadUserInfo: unit -> Promise<obj>

// [<Import("Keycloak","keycloak-js")>]
[<RequireQualifiedAccess>]
module Keycloak =
  [<Emit "new Keycloak($0)">]
  /// Create an instance of Keycloak
  /// @param: KeycloakConfig Configuration
  let create (config: KeycloakConfig): Keycloak = jsNative