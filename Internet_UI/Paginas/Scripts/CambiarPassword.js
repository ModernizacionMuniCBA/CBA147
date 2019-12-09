$(function () {
    $('.btn-volver').click(function () {
        redirigir('MiPerfil#contenedor_DatosAcceso');
    });
})

// Init
function init(data) {
    if (verificarError(data) == true) return;

    initCambiarPasswordEvent();
}


// Cambiar password
function initCambiarPasswordEvent() {
    $('.btn-guardar').click(cambiarPassword);
}

function cambiarPassword() {
    if (!validarCambiarPassword()) return;

    mostrarCargando(true);
    ocultarError('error_CambiarPassword');

    ajaxCambiarPassword(comandoCambiarPassword())
        .then(function () {
            redirigir('MiPerfil', { texto: 'Contraseña actualizada correctamente', tipo: 'exito' });
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarError('error_CambiarPassword', error);
        });
}

function ajaxCambiarPassword(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/CambiarPassword'),
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

function validarCambiarPassword() {
    let passwordAnterior = $('#input_PasswordAnterior').val();
    let PasswordNuevo = $('#input_PasswordNuevo').val();
    let PasswordNuevo2 = $('#input_PasswordNuevo2').val();

    if (passwordAnterior === '' || PasswordNuevo === '' || PasswordNuevo2 === '') {
        mostrarError('error_CambiarPassword', 'Complete todos los campos');
        return false;
    }

    if (PasswordNuevo.length < 8) {
        mostrarError('error_CambiarPassword', 'Ingrese una contraseña de 8 o más caracteres');
        return false;
    }

    if (PasswordNuevo !== PasswordNuevo2) {
        mostrarError('error_CambiarPassword', 'Las contraseñas ingresadas no coinciden');
        return false;
    }

    return true;
}

function comandoCambiarPassword() {
    let comando = {};
    comando.PasswordAnterior = $('#input_PasswordAnterior').val();
    comando.PasswordNuevo = $('#input_PasswordNuevo').val();
    return comando;
}