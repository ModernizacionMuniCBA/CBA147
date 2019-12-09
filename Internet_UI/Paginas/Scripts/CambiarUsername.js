$(function () {
    $('.btn-volver').click(function () {
        redirigir('MiPerfil#contenedor_DatosAcceso');
    });
})

// Init
function init(data) {
    if (verificarError(data) == true) return;

    initCambiarUsername();
    initCambiarUsernameEvent();
}

// Cambiar username
function initCambiarUsername() {
    if (usuarioLogeado.Username === usuarioLogeado.Cuil) {
        $('#input_UsernameActual').val(usuarioLogeado.Cuil);
    } else {
        $('#input_UsernameActual').val(usuarioLogeado.Username);
    }
}

function initCambiarUsernameEvent() {
    $('.btn-guardar').click(cambiarUsername);
}

function cambiarUsername() {
    if (!validarCambiarUsername()) return;

    mostrarCargando(true);
    ocultarError('error_CambiarUsername');

    ajaxCambiarUsername(comandoCambiarUsername())
        .then(function () {
            redirigir('MiPerfil', { texto: 'Nombre de usuario actualizado correctamente', tipo: 'exito' });
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarError('error_CambiarUsername', error);
        });
}

function ajaxCambiarUsername(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/CambiarUsername'),
            Data: { comando: comando },
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
        })
    });
}

function validarCambiarUsername() {
    let username = $('#input_Username').val();

    if (username === '') {
        mostrarError('error_CambiarUsername', 'Complete el nombre de usuario');
        return false;
    }

    if (username.length < 8) {
        mostrarError('error_CambiarUsername', 'Ingrese una nombre de usuario de 8 o más caracteres');
        return false;
    }

    return true;
}

function comandoCambiarUsername() {
    let comando = {};
    comando.Username = $('#input_Username').val();
    return comando;
}