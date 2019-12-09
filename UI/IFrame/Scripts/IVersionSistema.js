let callback;

function init(data) {
    $('#form').on('submit', function () {
        registrar();
        return false;
    });
}


function validarDatosFormulario() {
    return $('#form').valid();
}

function registrar() {
    if (!validarDatosFormulario()) return;

    mostrarCargando(true);

    var url = ResolveUrl('~/Servicios/VersionSistemaService.asmx/Insertar');

    crearAjax({
        Url: url,
        Data: {
            comando: {
                Version: $('#input_Version').val(),
                Descripcion: $('#input_Descripcion').val()
            }
        },
        OnSuccess: function (result) {
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "Versión registrada correctamente");
            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}


function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar() {
    if (callback == undefined) return;
    callback();
}