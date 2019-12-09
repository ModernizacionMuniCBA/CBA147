let callbackCambiar;

let estados;
let requerimiento;

function init(data) {
    if ('Error' in data) {
        return;
    }

    requerimiento = data.Requerimiento;

    //-----------------------------------
    // Estado
    //-----------------------------------
    estados = [];
    $.each(data.Estados, function (index, estado) {
        if (estado.KeyValue != requerimiento.Estado.Estado.KeyValue) {
            estados.push(estado);
        }
    });

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
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/CambiarEstado'),
        Data: { id: requerimiento.Id, keyValue: keyValue, observaciones: $('#input_Motivo').val().trim() },
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

    if (requerimiento == undefined || requerimiento.Id == undefined || requerimiento.Id <= 0) {
        mostrarMensajeAlerta('Requerimiento invalido');
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
