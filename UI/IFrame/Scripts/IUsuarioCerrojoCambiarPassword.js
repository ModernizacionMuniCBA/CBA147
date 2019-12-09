let callback;

function init(data) {
    if ('Error' in data) {
        return;
    }
}

function validar() {
    return $('#form').valid();
}

function cambiarPassword() {
    if (!validar()) return;

    let passAnterior = $('#input_PasswordAnterior').val();
    var passNueva = $('#input_Password').val();

    mostrarCargando(true);
    ajax_CambiarPassword(passAnterior, passNueva)
        .then(function () {
            informar();
        })
        .catch(function (error) {
            mostrarCargando(false);
            top.mostrarMensaje('Error', error);
        });
}


function ajax_CambiarPassword(passAnterior, passNueva) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/UsuarioService.asmx/CambiarPassword'),
            Data: { passwordAnterior: passAnterior, passwordNueva: passNueva },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback();
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });
}
//----------------------------
// Listener Registro
//----------------------------

function informar() {
    if (callback == undefined) return;
    callback();
}

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}