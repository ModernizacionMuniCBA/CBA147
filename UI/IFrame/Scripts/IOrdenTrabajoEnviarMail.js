
var idOrdenTRabajo;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idOrdenTRabajo = data.IdOrdenTrabajo;


}

var callback;
function setOnComprobanteReenviadoListener(listener) {
    callback = listener;
}

function informar() {
    if (callback == undefined) return;
    callback();
}

function validar() {
    var mail = $('#input_Email').val();
    if (mail == "") {
        $('#input_Email').focus();
        mostrarMensaje('Alerta', 'Debe ingresar una direccion de e-mail');
        return false;
    }

    let formValido = $('#form').valid();
    if (!formValido) {
        $('#input_Email').focus();
        return false;
    }

    return true;
}
function getDatosEnviarMail() {
    var datosMail = {};
    datosMail.Id = idOrdenTRabajo;
    datosMail.Email = $('#input_Email').val();
    return datosMail;
}
function reenviarComprobante() {
    if (!validar()) return;
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EnviarMail'),
        Data: { comando: getDatosEnviarMail() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}
