var ControlUsuarioDetalle_Usuario;

var ControlUsuarioDetalle_CallbackMensaje;
var ControlUsuarioDetalle_CallbackCargando;

$(function () {
    $('#ControlUsuarioDetalle_btnVerMas').click(function () {
        if (ControlUsuarioDetalle_Usuario == undefined) return;

        crearDialogoIFrame({
            Titulo: 'Detalle de Usuario',
            Url: ResolveUrl('~/IFrame/IUsuarioDetalle.aspx?Id=' + ControlUsuarioDetalle_Usuario.Id),
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

function ControlUsuarioDetalle_SetUsuario(usuario) {
    if (usuario == null) {
        usuario = undefined;
    }
    ControlUsuarioDetalle_Usuario = usuario;

    if (usuario == undefined) {
        $('#ControlUsuarioDetalle_textoNombre').text('Sin datos');
        $('#ControlUsuarioDetalle_textoNumeroDocumento').hide();
        //$('#ControlUsuarioDetalle_btnVerMas').hide();
    } else {
        //Nombre
        $('#ControlUsuarioDetalle_textoNombre').show();
        $('#ControlUsuarioDetalle_textoNombre').html(toTitleCase(usuario.Nombre  + ' ' + usuario.Apellido));

        //Nro Documento
        $('#ControlUsuarioDetalle_textoNumeroDocumento').show();
        $('#ControlUsuarioDetalle_textoNumeroDocumento').html('Usuario: '+usuario.Username);

        //$('#ControlUsuarioDetalle_btnVerMas').show();
    }
}

function ControlUsuarioDetalle_GetUsuario() {
    return ControlUsuarioDetalle_Usuario;
}


//-------------------------------
// Cargando
//-------------------------------

function ControlUsuarioDetalle_MostrarCargando(mostrar, mensaje) {
    if (ControlUsuarioDetalle_CallbackCargando == undefined) return;
    ControlUsuarioDetalle_CallbackCargando(mostrar, mensaje);
}

function ControlUsuarioDetalle_SetOnCargandoListener(callback) {
    ControlUsuarioDetalle_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function ControlUsuarioDetalle_SetOnMensajeListener(callback) {
    ControlUsuarioDetalle_CallbackMensaje = callback;

}

function ControlUsuarioDetalle_MostrarMensaje(tipo, mensaje) {
    if (ControlUsuarioDetalle_CallbackMensaje == undefined) return;
    ControlUsuarioDetalle_CallbackMensaje(tipo, mensaje);
}

function ControlUsuarioDetalle_MostrarMensajeError(mensaje) {
    ControlUsuarioDetalle_MostrarMensaje('Error', mensaje);
}

function ControlUsuarioDetalle_MostrarMensajeAlerta(mensaje) {
    ControlUsuarioDetalle_MostrarMensaje('Alerta', mensaje);
}

function ControlUsuarioDetalle_MostrarMensajeInfo(mensaje) {
    ControlUsuarioDetalle_MostrarMensaje('Info', mensaje);
}

function ControlUsuarioDetalle_MostrarMensajeExito(mensaje) {
    ControlUsuarioDetalle_MostrarMensaje('Exito', mensaje);
}