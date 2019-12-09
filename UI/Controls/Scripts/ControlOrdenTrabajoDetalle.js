var ControlOrdenTrabajoDetalle_OrdenTrabajo;

var ControlOrdenTrabajoDetalle_CallbackMensaje;
var ControlOrdenTrabajoDetalle_CallbackCargando;

$(function () {
    $('#ControlOrdenTrabajoDetalle_btnVerMas').click(function () {
        if (ControlOrdenTrabajoDetalle_OrdenTrabajo == undefined) return;

        crearDialogoIFrame({
            Titulo: 'Detalle de Orden de Trabajo',
            Url: ResolveUrl('~/IFrame/IOrdenTrabajoDetalle.aspx?Id=' + ControlOrdenTrabajoDetalle_OrdenTrabajo.Id),
            Botones:
                   [ {
                       Texto: 'Imprimir',
                       OnClick: function () {

                       }
                   },
                       {
                           Texto: 'Aceptar',
                           Class: 'colorExito'
                       }
                   ]
        });
    });
});

function ControlOrdenTrabajoDetalle_SetOrdenTrabajo(ot) {
    if (ot == null) {
        ot = undefined;
    }
    ControlOrdenTrabajoDetalle_OrdenTrabajo = ot;

    if (ot == undefined) {
        $('#ControlOrdenTrabajoDetalle_textoNumero').text('Sin datos');
        $('#ControlOrdenTrabajoDetalle_textoFechaAlta').hide();
        $('#ControlOrdenTrabajoDetalle_estado').hide();
        $('#ControlOrdenTrabajoDetalle_btnVerMas').hide();
    } else {
        //Numero
        $('#ControlOrdenTrabajoDetalle_textoNumero').show();
        $('#ControlOrdenTrabajoDetalle_textoNumero').html('Número: <b>'  + ot.Numero + '</b>');

        //Fecha Alta
        $('#ControlOrdenTrabajoDetalle_textoFechaAlta').show();
        $('#ControlOrdenTrabajoDetalle_textoFechaAlta').text(ot.FechaAltaString);

        //Estado
        $('#ControlOrdenTrabajoDetalle_estado').show();
        $('#ControlOrdenTrabajoDetalle_estado').find('.detalle').text(toTitleCase(ot.Estado.Nombre));
        $('#ControlOrdenTrabajoDetalle_estado').find('.indicador-estado').css('background-color', '#' + ot.Estado.Color);

        $('#ControlOrdenTrabajoDetalle_btnVerMas').show();

    }
}

function ControlOrdenTrabajoDetalle_GetOrdenTrabajo() {
    return ControlOrdenTrabajoDetalle_OrdenTrabajo;
}


//-------------------------------
// Cargando
//-------------------------------

function ControlOrdenTrabajoDetalle_MostrarCargando(mostrar, mensaje) {
    if (ControlOrdenTrabajoDetalle_CallbackCargando == undefined) return;
    ControlOrdenTrabajoDetalle_CallbackCargando(mostrar, mensaje);
}

function ControlOrdenTrabajoDetalle_SetOnCargandoListener(callback) {
    ControlOrdenTrabajoDetalle_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function ControlOrdenTrabajoDetalle_SetOnMensajeListener(callback) {
    ControlOrdenTrabajoDetalle_CallbackMensaje = callback;

}

function ControlOrdenTrabajoDetalle_MostrarMensaje(tipo, mensaje) {
    if (ControlOrdenTrabajoDetalle_CallbackMensaje == undefined) return;
    ControlOrdenTrabajoDetalle_CallbackMensaje(tipo, mensaje);
}

function ControlOrdenTrabajoDetalle_MostrarMensajeError(mensaje) {
    ControlOrdenTrabajoDetalle_MostrarMensaje('Error', mensaje);
}

function ControlOrdenTrabajoDetalle_MostrarMensajeAlerta(mensaje) {
    ControlOrdenTrabajoDetalle_MostrarMensaje('Alerta', mensaje);
}

function ControlOrdenTrabajoDetalle_MostrarMensajeInfo(mensaje) {
    ControlOrdenTrabajoDetalle_MostrarMensaje('Info', mensaje);
}

function ControlOrdenTrabajoDetalle_MostrarMensajeExito(mensaje) {
    ControlOrdenTrabajoDetalle_MostrarMensaje('Exito', mensaje);
}