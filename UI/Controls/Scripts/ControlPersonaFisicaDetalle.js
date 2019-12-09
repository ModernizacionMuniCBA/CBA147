var ControlPersonaFisicaDetalle_PersonaFisica;

var ControlPersonaFisicaDetalle_CallbackMensaje;
var ControlPersonaFisicaDetalle_CallbackCargando;

$(function () {
    $('#ControlPersonaFisicaDetalle_btnVerMas').click(function () {
        if (ControlPersonaFisicaDetalle_PersonaFisica == undefined) return;

        crearDialogoIFrame({
            Titulo: 'Detalle de Persona Física',
            Url: ResolveUrl('~/IFrame/IPersonaFisicaDetalle.aspx?Id=' + ControlPersonaFisicaDetalle_PersonaFisica.Id),
            Botones:
                   [
                       {
                           Texto: 'Aceptar',
                           Class: 'colorExito'
                       }
                   ]
        });
    });
});

function ControlPersonaFisicaDetalle_SetPersonaFisica(persona) {
    if (persona == null) {
        persona = undefined;
    }
    ControlPersonaFisicaDetalle_PersonaFisica = persona;

    if (persona == undefined) {
        $('#ControlPersonaFisicaDetalle_textoNombre').text('Sin datos');
        $('#ControlPersonaFisicaDetalle_textoNumeroDocumento').hide();
        $('#ControlPersonaFisicaDetalle_btnVerMas').hide();
    } else {
        //Nombre
        $('#ControlPersonaFisicaDetalle_textoNombre').show();
        $('#ControlPersonaFisicaDetalle_textoNombre').html(toTitleCase(persona.Nombre  + ' ' + persona.Apellido));

        //Nro Documento
        $('#ControlPersonaFisicaDetalle_textoNumeroDocumento').show();
        $('#ControlPersonaFisicaDetalle_textoNumeroDocumento').html(persona.TipoDocumentoString + ': ' + persona.NroDoc);

        $('#ControlPersonaFisicaDetalle_btnVerMas').show();
    }
}

function ControlPersonaFisicaDetalle_GetPersonaFisica() {
    return ControlPersonaFisicaDetalle_PersonaFisica;
}


//-------------------------------
// Cargando
//-------------------------------

function ControlPersonaFisicaDetalle_MostrarCargando(mostrar, mensaje) {
    if (ControlPersonaFisicaDetalle_CallbackCargando == undefined) return;
    ControlPersonaFisicaDetalle_CallbackCargando(mostrar, mensaje);
}

function ControlPersonaFisicaDetalle_SetOnCargandoListener(callback) {
    ControlPersonaFisicaDetalle_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function ControlPersonaFisicaDetalle_SetOnMensajeListener(callback) {
    ControlPersonaFisicaDetalle_CallbackMensaje = callback;

}

function ControlPersonaFisicaDetalle_MostrarMensaje(tipo, mensaje) {
    if (ControlPersonaFisicaDetalle_CallbackMensaje == undefined) return;
    ControlPersonaFisicaDetalle_CallbackMensaje(tipo, mensaje);
}

function ControlPersonaFisicaDetalle_MostrarMensajeError(mensaje) {
    ControlPersonaFisicaDetalle_MostrarMensaje('Error', mensaje);
}

function ControlPersonaFisicaDetalle_MostrarMensajeAlerta(mensaje) {
    ControlPersonaFisicaDetalle_MostrarMensaje('Alerta', mensaje);
}

function ControlPersonaFisicaDetalle_MostrarMensajeInfo(mensaje) {
    ControlPersonaFisicaDetalle_MostrarMensaje('Info', mensaje);
}

function ControlPersonaFisicaDetalle_MostrarMensajeExito(mensaje) {
    ControlPersonaFisicaDetalle_MostrarMensaje('Exito', mensaje);
}