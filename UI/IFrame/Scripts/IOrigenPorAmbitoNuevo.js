
var callback;
var ambitos;
var origenes;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    ambitos = data.Ambitos;
    origenes = data.Origenes;

    $("#inputFormulario_SelectAmbito").CargarSelect({
        Data: ambitos,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    $("#inputFormulario_SelectOrigen").CargarSelect({
        Data: origenes,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    var idAmbito = $.url().param('IdAmbito');
    if (idAmbito != undefined) {
        $("#inputFormulario_SelectAmbito").val(idAmbito);
        $("#inputFormulario_SelectAmbito").trigger('change');
    }

    var idOrigen = $.url().param('IdOrigen');
    if (idOrigen != undefined) {
        $("#inputFormulario_SelectOrigen").val(idOrigen);
        $("#inputFormulario_SelectOrigen").trigger('change');
    }

}

//-----------------------------
// Operaciones globales 
//-----------------------------
function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando origen...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrigenPorAmbitoService.asmx/Insertar'),
        Data: { 'comando': getEntity() },
        OnSuccess: function (resultado) {
            mostrarCargando(false);

            if (!resultado.Ok) {
                mostrarMensaje('Error', resultado.Error);
                return;
            }

            informar(resultado.Return);
        },
        OnError: function (resultado) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    $('.control-observacion').text('');

    var resultado = true;

    //Area
    var idAmbito = $('#inputFormulario_SelectAmbito').val();
    if (idAmbito == undefined || idAmbito == -1) {
        $('#inputFormulario_SelectAmbito').siblings('.control-observacion').text('Dato requerido');
        resultado = false;
    }

    //Origen
    var idOrigen = $('#inputFormulario_SelectOrigen').val();
    if (idOrigen == undefined || idOrigen == -1) {
        $('#inputFormulario_SelectOrigen').siblings('.control-observacion').text('Dato requerido');
        resultado = false;
    }

    return resultado;
}

function getEntity() {
    var entity = {};
    entity.AmbitoId = $('#inputFormulario_SelectAmbito').val();
    entity.OrigenId = $('#inputFormulario_SelectOrigen').val();
    return entity;
}


function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}