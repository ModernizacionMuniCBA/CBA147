

function init(data) {

    data = parse(data);

    $('#input_MailContacto').val(data.Usuario.Email);
    $('#input_TelefonoContacto').val(data.Usuario.Telefono);

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

    var data = {
        mensaje: $('#input_Descripcion').val(),
        mail: $('#input_MailContacto').val(),
        telefono: $('#input_TelefonoContacto').val()
    }
    data = JSON.stringify(data);

    //Muestro cargando
    $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(500);

    var url = ResolveUrl('~/Servicios/ContactoService.asmx/EnviarMailContacto');

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: data,
        url: url,
        success: function (result) {
            result = parse(result.d);

            //Oculto el cargando
            $('#cardFormulario').find('.cargando').stop(true, true).fadeOut(500);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }
            mostrarMensaje('Exito', "Email enviado correctamente");
            limpiarCampos();
        },
        error: function (result) {
            $('#cardFormulario').find('.cargando').stop(true, true).fadeOut(500);
            mostrarMensaje('Error', 'Error al enviar mail');
        }
    });
}


function limpiarCampos() {
    $('#input_Descripcion').val('');
    $('#input_MailContacto').val('');
    $('#input_TelefonoContacto').val('');
    Materialize.updateTextFields();
}

