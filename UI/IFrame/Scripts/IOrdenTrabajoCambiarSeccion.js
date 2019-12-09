let callbackCambiar;

let secciones;
let idOT;

function init(data) {
    if ('Error' in data) {
        return;
    }

    secciones = data.Secciones;
    idOT = data.IdOrdenTrabajo;
    idSeccion = data.IdSeccion;

    $('#select_Seccion').CargarSelect({
        Data: secciones,
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Ninguna',
        Sort: true
    });

    if (idSeccion != 0)
        $('#select_Seccion').val(idSeccion).trigger('change');
}

function cambiarSeccion() {
    let seccionSeleccionada = $('#select_Seccion').val();
    let mensaje = 'Cambiando sección...';

    if (seccionSeleccionada == -1) {
        mensaje = "Eliminando sección...";
    }
    mostrarCargando(true, mensaje);

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/CambiarSeccion'),
        Data: { comando: { IdOrdenTrabajo: idOT, IdSeccion: seccionSeleccionada } },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje(result.Error);
                return;
            }

            informarCambioSeccion(idOT);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

//-------------------------------
// Listener
//-------------------------------

function setCallback(callback) {
    callbackCambiar = callback;
}

function informarCambioSeccion(id) {
    if (callbackCambiar == undefined) return;
    callbackCambiar(id);
}