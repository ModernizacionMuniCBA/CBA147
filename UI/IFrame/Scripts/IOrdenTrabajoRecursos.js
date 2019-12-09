let ot;
let callback;

function init(data) {
    ot = data.OrdenTrabajo;

    $('#input_Material').val(ot.RecursoMaterial);
    $('#input_Personal').val(ot.RecursoPersonal);
    Materialize.updateTextFields();
}

function guardar() {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EditarRecursos'),
        Data: {
            comando: {
                IdOrdenTrabajo: ot.Id,
                Personal: $('#input_Personal').val(),
                Material: $('#input_Material').val()
            }
        },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje(result.Error);
                return;
            }

            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error procesando la solicitud');
        }
    })
}

//----------------------------
// Listener
//----------------------------

function informar() {
    if (callback == null) return;
    callback();
}

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}