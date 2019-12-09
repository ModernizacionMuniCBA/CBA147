var ControlRequerimientoDetalle_Requerimiento;

var ControlRequerimientoDetalle_CallbackMensaje;
var ControlRequerimientoDetalle_CallbackCargando;

$(function () {
    $('#ControlRequerimientoDetalle_btnVerMas').click(function () {
        if (ControlRequerimientoDetalle_Requerimiento == undefined) return;

        crearDialogoDetalleRequerimiento({
            Id: ControlRequerimientoDetalle_Requerimiento.Id,
            CallbackMensajes: function (tipo, mensaje) {
                ControlRequerimientoDetalle_MostrarMensaje(tipo, mensaje);
            },
            CallbackCargando: function (cargando, mensaje) {
                ControlRequerimientoDetalle_MostrarCargando(cargando, mensaje);
            }
        });
    });
});

function ControlRequerimientoDetalle_SetRequerimiento(rq) {
    if (rq == null) {
        rq = undefined;
    }
    ControlRequerimientoDetalle_Requerimiento = rq;

    if (rq == undefined) {
        $('#ControlRequerimientoDetalle_textoNumero').text('Sin datos');
        $('#ControlRequerimientoDetalle_textoFechaAlta').hide();
        $('#ControlRequerimientoDetalle_estado').hide();
        $('#ControlRequerimientoDetalle_btnVerMas').hide();
    } else {
        //Numero
        $('#ControlRequerimientoDetalle_textoNumero').show();
        $('#ControlRequerimientoDetalle_textoNumero').html(rq.Numero);

        //Fecha Alta
        $('#ControlRequerimientoDetalle_textoFechaAlta').show();
        $('#ControlRequerimientoDetalle_textoFechaAlta').text(dateTimeToString(rq.FechaAlta));

        //Estado
        $('#ControlRequerimientoDetalle_estado').show();
        $('#ControlRequerimientoDetalle_estado').find('.detalle').text(toTitleCase(rq.Estado.Estado.Nombre));
        $('#ControlRequerimientoDetalle_estado').find('.indicador-estado').css('background-color', '#' + rq.Estado.Color);

        $('#ControlRequerimientoDetalle_btnVerMas').show();
    }
}

function ControlRequerimientoDetalle_GetRequerimiento() {
    return ControlRequerimientoDetalle_Requerimiento;
}

//-------------------------------
// Cargando
//-------------------------------

function ControlRequerimientoDetalle_MostrarCargando(mostrar, mensaje) {
    if (ControlRequerimientoDetalle_CallbackCargando == undefined) return;
    ControlRequerimientoDetalle_CallbackCargando(mostrar, mensaje);
}

function ControlRequerimientoDetalle_SetOnCargandoListener(callback) {
    ControlRequerimientoDetalle_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function ControlRequerimientoDetalle_SetOnMensajeListener(callback) {
    ControlRequerimientoDetalle_CallbackMensaje = callback;

}

function ControlRequerimientoDetalle_MostrarMensaje(tipo, mensaje) {
    if (ControlRequerimientoDetalle_CallbackMensaje == undefined) return;
    ControlRequerimientoDetalle_CallbackMensaje(tipo, mensaje);
}

function ControlRequerimientoDetalle_MostrarMensajeError(mensaje) {
    ControlRequerimientoDetalle_MostrarMensaje('Error', mensaje);
}

function ControlRequerimientoDetalle_MostrarMensajeAlerta(mensaje) {
    ControlRequerimientoDetalle_MostrarMensaje('Alerta', mensaje);
}

function ControlRequerimientoDetalle_MostrarMensajeInfo(mensaje) {
    ControlRequerimientoDetalle_MostrarMensaje('Info', mensaje);
}

function ControlRequerimientoDetalle_MostrarMensajeExito(mensaje) {
    ControlRequerimientoDetalle_MostrarMensaje('Exito', mensaje);
}