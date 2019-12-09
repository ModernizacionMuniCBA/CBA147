var callback;
var idRq;
var desdeOT = false;
var tipoMotivo;

function init(data) {
    if ('Error' in data) {
        return;
    }

    //requerimiento = data.Requerimiento;
    idRq = data.IdRequerimiento;
    desdeOT = data.DesdeOT;
    tipoMotivo = data.TipoMotivo;

    //-----------------------------------
    // Motivo
    //-----------------------------------
    SelectorMotivo_Init({
        TipoMotivo: tipoMotivo,
        MostrarTiposMotivo:true,
        ModoBusqueda: true,
        CallbackMensaje: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function (motivo) {
            if (motivo == undefined) {
                $('#contenedor_Area').removeClass('visible');
            } else {
                $('#contenedor_Area').addClass('visible');
                $('#texto_Area').text(toTitleCase(motivo.NombreArea));
            }
        }
    });

    let textoDescripcion = "Desde esta pantalla usted puede editar el servicio y motivo de su requerimiento.<br/>Tenga en cuenta que al hacerlo, es posible que éste se mueva a otra Área Operativa";
    if (desdeOT) {
        textoDescripcion = "Desde esta pantalla usted puede editar el servicio y motivo de su requerimiento.<br/>Tenga en cuenta que al hacerlo, éste se quitará de la orden de trabajo y es posible que se mueva a otra Área Operativa";
    }

    $("#info label").html(textoDescripcion);
}

function guardar() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Cambiando el motivo del requerimiento...');

    let idMotivo = SelectorMotivo_GetMotivoSeleccionado().Id;
    var url = '~/Servicios/RequerimientoService.asmx/CambiarMotivo';
    if (desdeOT) {
        url = '~/Servicios/RequerimientoService.asmx/CambiarMotivoDesdeOT';
    }

    crearAjax({
        Url: ResolveUrl(url),
        Data: { id: idRq, idMotivo: idMotivo },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error',result.Error);
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

    //Oculto los errores
    $('.control-observacion').stop(true, true).slideUp(300);

    if (idRq == undefined || idRq <= 0) {
        mostrarMensajeAlerta('Requerimiento invalido');
        return false;
    }

    
    if (SelectorMotivo_GetServicioSeleccionado() == undefined) {
        mostrarMensaje('Alerta', 'Debe seleccionar un servicio');
        return false;
    }

    if (SelectorMotivo_GetMotivoSeleccionado() == undefined) {
        mostrarMensaje('Alerta', 'Debe seleccionar un motivo');
        return false;
    }

    return true;
}

//-------------------------------
// Listener
//-------------------------------

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(result) {
    if (callback == undefined) return;
    callback(result);
}