export var initKeycloak = () => {
    var keycloak = new Keycloak({
        url: 'http://localhost:8080/auth',
        realm: 'demo',
        clientId: 'fable-react-client'
    });
    keycloak.init({ promiseType: 'native', onLoad: 'login-required' }).then(function (authenticated) {
        console.log(authenticated ? 'authenticated' : 'not authenticated');
    }).catch(function () {
        console.log('failed to initialize');
    });
};
