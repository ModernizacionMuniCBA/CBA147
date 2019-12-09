
var callback;
var origenes;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    origenes = data.Origenes;

    SelectorUsuario_SetOnCargandoListener(function (cargando, mensaje) {
        mostrarCargando(cargando, mensaje);
    });

    SelectorUsuario_SetVisibleNuevoUsuario(false);
    if (data.Usuario != undefined) {
        SelectorUsuario_SetUsuario(data.Usuario);
    }

    $("#inputFormulario_SelectOrigen").CargarSelect({
        Data: origenes,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });


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
        Url: ResolveUrl('~/Servicios/OrigenPorUsuarioService.asmx/Insertar'),
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

    if (SelectorUsuario_GetUsuarioSeleccionado() == undefined) {
        mostrarMensaje('Alerta', 'Debe seleccionar un usuario');
        return false;
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
    entity.UsuarioId = SelectorUsuario_GetUsuarioSeleccionado().Id;
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