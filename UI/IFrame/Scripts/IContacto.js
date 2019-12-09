let callback;

function init(data) {
    let email = getUsuarioLogeado().Usuario.Email != undefined ? getUsuarioLogeado().Usuario.Email : '';
    let telefono = getUsuarioLogeado().Usuario.Telefono != undefined ? getUsuarioLogeado().Usuario.Telefono : '';
    $('#input_MailContacto').val(email);
    $('#input_TelefonoContacto').val(telefono);
    Materialize.updateTextFields();

    $('#cardFormulario').find('.contenedor-footer').find('.btnOk').click(function () {
        enviarMail();
    });

    $('#formContacto').on('submit', function () {
        enviarMail();
        return false;
    });
}


function validarDatosFormularioMail() {
    return $('#formContacto').valid();
}

function enviarMail() {
    if (!validarDatosFormularioMail()) return;

    mostrarCargando(true);

    var data = {
        mensaje: $('#input_Descripcion').val(),
        mail: $('#input_MailContacto').val(),
        telefono: $('#input_TelefonoContacto').val()
    }

    var url = ResolveUrl('~/Servicios/ContactoService.asmx/EnviarMailContacto');

    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "Email enviado correctamente");
            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}

function limpiarCampos() {
    $('#input_Descripcion').val('');
    $('#input_MailContacto').val('');
    $('#input_TelefonoContacto').val('');
    Materialize.updateTextFields();
}



function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar() {
    if (callback == undefined) return;
    callback();
}