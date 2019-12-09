let calback;

let estados;
let requerimiento;
let ordenTrabajo;
let ordenInspeccion;

var modo;
var modo_OT='OrdenTrabajo';
var modo_OI='OrdenInspeccion';

function init(data) {
    if ('Error' in data) {
        mostrarMensajeCritico({ Icono: 'error_outline', Titulo: data.Error });
        return;
    }


    requerimiento = data.Requerimiento;
    if(data.OrdenTrabajo!=undefined){
        ordenTrabajo = data.OrdenTrabajo;
        modo = modo_OT;
        $("#contenedorCheckboxDesmarcar").hide();
    }

    if (data.OrdenInspeccion != undefined) {
        ordenInspeccion = data.OrdenInspeccion;
        modo = modo_OI;
        $("#check_Desmarcar").prop("checked", true);
    }

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

function quitar() {
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

    var idOT=null;
    var idOI=null;
    var url='';

    if(modo==modo_OT){
        url=ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/QuitarRequerimiento');
        idOT=ordenTrabajo.Id;
    } else {
        url= ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/QuitarRequerimiento');
        idOI=ordenInspeccion.Id;
    }

    var data = {
        comando: {
            IdOrdenTrabajo: idOT,
            IdOrdenInspeccion: idOI,
            IdRequerimiento: requerimiento.Id,
            Desmarcar: $("#check_Desmarcar").is(':checked'),
            EstadoKeyValue: keyValue,
            Observaciones: $('#input_Motivo').val().trim()
        }
    };

    crearAjax({
        Url:url,
        Data: data,
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar();
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

    if (modo == modo_OT && (ordenTrabajo == undefined || ordenTrabajo.Id == undefined || ordenTrabajo.Id <= 0)) {
        mostrarMensajeAlerta('Orden de trabajo inválida');
        return false;
    }

    if (modo == modo_OI && (ordenInspeccion == undefined || ordenInspeccion.Id == undefined || ordenInspeccion.Id <= 0)) {
        mostrarMensajeAlerta('Orden de inspección inválida');
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

function setCallback(callbackNuevo) {
    calback = callbackNuevo;
}

function informar() {
    if (calback == undefined) return;
    calback();
}