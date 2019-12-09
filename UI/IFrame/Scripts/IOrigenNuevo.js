var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var origen;
var modo;

var callback;

function init(data) {
    origen = data.Origen;

    if (origen == undefined) {
        modo = modoRegistrar;
    }
    else {
        modo = modoEditar;
    }

    switch (modo) {
        case modoRegistrar:
            break;

        case modoEditar:
            cargarDatos();
            break;
    }
}

function cargarDatos() {
    $('#inputFormulario_Nombre').val(origen.Nombre);
    Materialize.updateTextFields();
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
        Url: ResolveUrl('~/Servicios/OrigenService.asmx/Insertar'),
        Data: { 'comando': getOrigen() },
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

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando origen...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrigenService.asmx/Editar'),
        Data: { 'comando': getOrigen() },
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
    $('#cardFormulario').find('.control-observacion').text('');

    var resultado = true;

    //Nombre
    var nombre = $('#inputFormulario_Nombre').val();
    if (nombre == null || nombre == "") {
        $('#errorFormulario_Nombre').text('Dato requerido');
        resultado = false;
    }
    return resultado;
}

function getOrigen() {

    //Declaro un objeto
    var entity = {};
    if (modo == modoEditar) {
        entity.Id = origen.Id;
    }

    var nombre = $('#inputFormulario_Nombre').val();
    entity.Nombre = '' + nombre;
    return entity;
}


function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}