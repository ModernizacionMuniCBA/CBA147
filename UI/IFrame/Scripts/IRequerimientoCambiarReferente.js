var callback;
var idRequerimiento;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idRequerimiento = data.IdRequerimiento;

    //--------------------------------
    // Referente
    //--------------------------------

    SelectorUsuario_SetOnCargandoListener(function (mostrar, mensaje) {
        mostrarCargando(mostrar, mensaje);
    });

    SelectorUsuario_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    })
}

function cambiarReferente() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Cambiando referente...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/AgregarReferente'),
        Data: { id: idRequerimiento, idUsuario: SelectorUsuario_GetUsuarioSeleccionado().Id },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje("Error",result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    var valido = true;

    //Oculto los errores
    $('.control-observacion').stop(true, true).slideUp(300);

    if (idRequerimiento == undefined || idRequerimiento <= 0) {
        mostrarMensajeAlerta('Requerimiento invalido');
        return false;
    }

    if (SelectorUsuario_GetUsuarioSeleccionado() == undefined) {
        mostrarMensajeAlerta('Debe seleccionar un usuario');
        return false;
    }

    return valido;
}

//-------------------------------
// Listener
//-------------------------------

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(usuario) {
    if (callback == undefined) return;
    callback(usuario);
}