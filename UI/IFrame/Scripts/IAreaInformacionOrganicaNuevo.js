let callback;

function init(data) {

    $('#form').on('submit', function () {
        guardar();
        return false;
    });
}

function validar() {
    return $('#form').valid();
}

function guardar() {
    if (!validar()) return;

    mostrarCargando(true);

    var data = {

    }

    var url = ResolveUrl('~/Servicios/AreaService.asmx/SetInformacionOrganica');

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

            mostrarMensaje('Exito', "Informacion organica actualizada");
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