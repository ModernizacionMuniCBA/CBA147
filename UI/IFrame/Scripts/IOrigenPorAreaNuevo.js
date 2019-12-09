
var callback;
var areas;
var origenes;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    areas = data.Areas;
    origenes = data.Origenes;

    $("#inputFormulario_SelectArea").CargarSelect({
        Data: areas,
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

    var idArea = $.url().param('IdArea');
    if (idArea != undefined) {
        $("#inputFormulario_SelectArea").val(idArea);
        $("#inputFormulario_SelectArea").trigger('change');
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
        Url: ResolveUrl('~/Servicios/OrigenPorAreaService.asmx/Insertar'),
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
    var idArea = $('#inputFormulario_SelectArea').val();
    if (idArea == undefined || idArea == -1) {
        $('#inputFormulario_SelectArea').siblings('.control-observacion').text('Dato requerido');
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
    entity.AreaId = $('#inputFormulario_SelectArea').val();
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