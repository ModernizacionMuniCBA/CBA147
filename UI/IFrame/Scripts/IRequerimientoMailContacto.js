var mail;
var idRequerimiento;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idRequerimiento = data.IdRequerimiento;
}

var callback;
function setOnEnviadoListener(listener) {
    callback = listener;
}

function informar() {
    if (callback == undefined) return;
    callback();
}

function validar() {
     var contenido = $('#input_Mensaje').val();
    if (contenido == "") {
        $('#input_Mensaje').focus();
        mostrarMensaje('Alerta', 'Debe ingresar el contenido del e-mail');
        return false;
    }
    return true;
}

function enviar() {
    if (!validar()) return;
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EnviarMensaje'),
        Data: { id: idRequerimiento, mensaje: $('#input_Mensaje').val() },
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