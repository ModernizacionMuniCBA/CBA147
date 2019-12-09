var ControlDomicilioDetalle_Domicilio;

var ControlDomicilioDetalle_CallbackMensaje;
var ControlDomicilioDetalle_CallbackCargando;

$(function () {
    //$('#ControlDomicilioDetalle_btnVerMas').hide();
    $('#ControlDomicilioDetalle_btnVerMas').click(function () {
        if (ControlDomicilioDetalle_Domicilio == undefined) return;

        crearDialogoIFrame({
            Titulo: '<label>Detalle de Domicilio</label>',
            Url: ResolveUrl('~/IFrame/DomicilioDetalle.aspx?Id=' + ControlDomicilioDetalle_Domicilio.Id),
            Botones:
                   [
                       {
                           Texto: 'Mapa',
                           CerrarDialogo: false,
                           OnClick: function () {
                               crearDialogoMapaGoogleByIdDomicilio({
                                   Id: ControlDomicilioDetalle_Domicilio.Id,
                                   CallbackMensajes: function (tipo, mensaje) {
                                       ControlDomicilioDetalle_MostrarMensaje(tipo, mensaje);
                                   },
                                   CallbackCargando: function (mostrar, mensaje) {
                                       ControlDomicilioDetalle_MostrarCargando(mostrar, mensaje);
                                   }
                               });
                           }
                       },
                       {
                           Texto: 'Aceptar',
                           Class: 'colorExito'
                       }
                   ]
        });
    });

    $('#ControlDomicilioDetalle_btnMapa').click(function () {
        if (ControlDomicilioDetalle_Domicilio == undefined) return;

        crearDialogoMapaGoogleByIdDomicilio({
            Id: ControlDomicilioDetalle_Domicilio.Id,
            CallbackMensajes: function (tipo, mensaje) {
                ControlDomicilioDetalle_MostrarMensaje(tipo, mensaje);
            },
            CallbackCargando: function (mostrar, mensaje) {
                ControlDomicilioDetalle_MostrarCargando(mostrar, mensaje);
            }
        });
    });
});

function ControlDomicilioDetalle_SetDomicilio(domicilio) {
    if (domicilio == null) {
        domicilio = undefined;
    }
    ControlDomicilioDetalle_Domicilio = domicilio;

    if (domicilio == undefined || domicilio == null) {
        $('#ControlDomicilioDetalle_btnVerMas').hide();
        $('#ControlDomicilioDetalle_textoTitulo').text('Sin datos');
    } else {
        if (typeof domicilio == "string") {
            $('#ControlDomicilioDetalle_btnVerMas').hide();
            $('#ControlDomicilioDetalle_textoTitulo').text(ControlDomicilioDetalle_Domicilio);
        } else {
            $('#ControlDomicilioDetalle_btnVerMas').show();

            //Calle
            if (ControlDomicilioDetalle_Domicilio.Calle != null) {
                $('#ControlDomicilioDetalle_textoTitulo').show();
                $('#ControlDomicilioDetalle_textoTitulo').text(toTitleCase(ControlDomicilioDetalle_Domicilio.Calle.Nombre) + ' ' + ControlDomicilioDetalle_Domicilio.Altura);
            } else {
                $('#ControlDomicilioDetalle_textoTitulo').hide();
            }

            //Barrio
            if (ControlDomicilioDetalle_Domicilio.Barrio != null) {
                $('#ControlDomicilioDetalle_textoBarrio').show();
                $('#ControlDomicilioDetalle_textoBarrio').text('Barrio: ' + toTitleCase(ControlDomicilioDetalle_Domicilio.Barrio.Nombre));
            } else {
                $('#ControlDomicilioDetalle_textoBarrio').hide();
            }

            //CPC
            if (ControlDomicilioDetalle_Domicilio.Cpc != null) {
                $('#ControlDomicilioDetalle_textoCPC').show();
                $('#ControlDomicilioDetalle_textoCPC').text('CPC: ' + toTitleCase(ControlDomicilioDetalle_Domicilio.Cpc.Nombre));
            } else {
                $('#ControlDomicilioDetalle_textoCPC').hide();
            }
        }
    }
}

function ControlDomicilio_GetDomicilio() {
    return ControlDomicilioDetalle_Domicilio;
}


//-------------------------------
// Cargando
//-------------------------------

function ControlDomicilioDetalle_MostrarCargando(mostrar, mensaje) {
    if (ControlDomicilioDetalle_CallbackCargando == undefined) return;
    ControlDomicilioDetalle_CallbackCargando(mostrar, mensaje);
}

function ControlDomicilioDetalle_SetOnCargandoListener(callback) {
    ControlDomicilioDetalle_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function ControlDomicilioDetalle_SetOnMensajeListener(callback) {
    ControlDomicilioDetalle_CallbackMensaje = callback;
}

function ControlDomicilioDetalle_MostrarMensaje(tipo, mensaje) {
    if (ControlDomicilioDetalle_CallbackMensaje == undefined) return;
    ControlDomicilioDetalle_CallbackMensaje(tipo, mensaje);
}

function ControlDomicilioDetalle_MostrarMensajeError(mensaje) {
    if (ControlDomicilioDetalle_CallbackMensaje == undefined) return;
    ControlDomicilioDetalle_CallbackMensaje('Error', mensaje);
}

function ControlDomicilioDetalle_MostrarMensajeAlerta(mensaje) {
    if (ControlDomicilioDetalle_CallbackMensaje == undefined) return;
    ControlDomicilioDetalle_CallbackMensaje('Alerta', mensaje);
}

function ControlDomicilioDetalle_MostrarMensajeInfo(mensaje) {
    if (ControlDomicilioDetalle_CallbackMensaje == undefined) return;
    ControlDomicilioDetalle_CallbackMensaje('Info', mensaje);
}

function ControlDomicilioDetalle_MostrarMensajeExito(mensaje) {
    if (ControlDomicilioDetalle_CallbackMensaje == undefined) return;
    ControlDomicilioDetalle_CallbackMensaje('Exito', mensaje);
}