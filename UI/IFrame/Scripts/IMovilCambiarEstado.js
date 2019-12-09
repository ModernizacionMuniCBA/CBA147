let callbackCambiar;

let estados;

let idEstadoAnterior;
let nombreEstadoAnterior;
let idEntidad;

let url;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idEstadoAnterior = data.IdEstadoAnterior;
    nombreEstadoAnterior = data.NombreEstadoAnterior;
    idEntidad = data.Id;
    estados = data.Estados;

    switch(data.Modo){
        case "Movil":
            url=ResolveUrl('~/Servicios/MovilService.asmx/CambiarEstado');
            break;
        case "Flota":
            url=ResolveUrl('~/Servicios/FlotaService.asmx/CambiarEstado');
            break;
    }

    //-----------------------------------
    // Estado
    //-----------------------------------
    estados.sort(function (a, b) {
        if (a.Nombre < b.Nombre) return -1;
        if (a.Nombre > b.Nombre) return 1;
        return 0;
    });

    $.each(estados, function (index, data) {
        data.html = '<div><div class="    display: flex !important; "><label class="indicador-estado" style="background-color: #' + data.Color + '"></label><span>' + toTitleCase(data.Nombre) + '</span></div></div>';
    });

    $('#select_Estado').CargarSelect({
        Data: estados,
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Seleccione...',
        Sort: false
    });
}

function cambiarEstado() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Cambiando estado...');


    let keyValue = $.grep(estados, function (element, index) {
        return element.Id == $('#select_Estado').val();
    })[0].KeyValue;

    if (keyValue == undefined) {
        mostrarMensaje('Error', 'Error con el estado');
        return;
    }

    crearAjax({
        Url: url,
        Data: { comando: { Id: idEntidad, EstadoKeyValue: keyValue, Observaciones: $('#input_Motivo').val().trim() } },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje(result.Error);
                return;
            }

            informarCambioEstado(result.Return);
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

    if (idEntidad == undefined || idEntidad <= 0) {
        mostrarMensajeAlerta('Entidad invalido');
        return false;
    }

    //Valido el estado
    if ($('#select_Estado').val() == -1) {
        $('#select_Estado').siblings('.control-observacion').text('Dato requerido');
        $('#select_Estado').siblings('.control-observacion').stop(true, true).slideDown(300);
        valido = false;
    }

    //Valido el motivo
    if ($('#input_Motivo').val().trim() == "") {
        $('#input_Motivo').siblings('.control-observacion').text('Dato requerido');
        $('#input_Motivo').siblings('.control-observacion').stop(true, true).slideDown(300);
        valido = false;
    } else {
        if ($('#input_Motivo').val().trim().length > 1000) {
            $('#input_Motivo').siblings('.control-observacion').text('Debe tener menos de 1000 caracteres');
            $('#input_Motivo').siblings('.control-observacion').stop(true, true).slideDown(300);
            valido = false;
        }
    }

    return valido;
}

//-------------------------------
// Listener
//-------------------------------

function setOnEstadoCambiadoListener(callback) {
    callbackCambiar = callback;
}

function informarCambioEstado(requerimiento, estado) {
    if (callbackCambiar == undefined) return;
    callbackCambiar(requerimiento, estado);
}