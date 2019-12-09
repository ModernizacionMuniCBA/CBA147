var mail;
var idRequerimiento;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idRequerimiento = data.IdRequerimiento;
    mail = data.Email;

    if (mail == undefined) {
        $('#indicadorSinMail').show();
    } else {
        $('#indicadorSinMail').hide();

        $('#input_Email').val(mail);
        Materialize.updateTextFields();
    }
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

function reenviarComprobante() {
    if (!validar()) return;
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EnviarComprobanteAtencion'),
        Data: { id: idRequerimiento, idUsuario: usuarioLogeado.Usuario.Id, mail: $('#input_Email').val() },
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
