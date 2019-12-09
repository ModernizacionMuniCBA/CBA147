var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;
var idEditando;

var callbackGuardar;
var callbackEditar;
var callbackMensaje;
var callbackCargando;

$(function () {
    init();
});


function init() {

    idEditando = $.url().param('Id');

    if (idEditando == undefined) {
        modo = modoRegistrar;
    }
    else {
        modo = modoEditar;
    }

    switch (modo) {
        case modoRegistrar:
            break;

        case modoEditar:
            setTimeout(function () {
                cargarArea();
            }, 100);
            break;
    }
}

//-----------------------------
// Operaciones globales 
//-----------------------------
function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando Area...');

    PageMethods.Insertar(getArea(), onSuccess, onError);
    function onSuccess(result) {

        mostrarCargando(false);

        if ('Error' in result) {
            mostrarMensajeError(result.Error);
            return;
        }

        if (callbackGuardar != undefined) {
            callbackGuardar(result.Area);
        }
    };

    function onError(result) {
        mostrarCargando(false);
        mostrarMensajeError(result.get_message());
    };
}

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando Area...');

    PageMethods.Actualizar(getArea(), onSuccess, onError);
    function onSuccess(result) {
        mostrarCargando(false);

        if ('Error' in result) {
            mostrarMensajeError(result.Error);
            return;
        }

        if (callbackEditar != undefined) {
            callbackEditar(result.Area);
        }
    };

    function onError(result) {
        mostrarCargando(false);
        mostrarMensajeError(result.get_message());
    };
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


function getArea() {

    //Declaro un objeto
    var serv = {};
    if (modo == modoEditar) {
        serv.Id = '' + idEditando;
    }

    var nombre = $('#inputFormulario_Nombre').val();
    serv.Nombre = '' + nombre;

    if ($('#rdbActivoNo').is(":checked")) {
        serv.Activo = false;
    } else {
        serv.Activo = true;
    }

    var obs = $('#inputFormulario_Observaciones').val();
    serv.Observaciones = '' + obs;
    return serv;
}

function reiniciarUI() {
    $($('#cardFormulario').find('.contenedor-main')).scrollTop(0);
    $('#inputFormulario_Observaciones').val('');
    $('#inputFormulario_Nombre').val('');
    Materialize.updateTextFields();
}

function cargarArea() {
    PageMethods.GetAreaById(idEditando, onSucces, onError);

    function onSucces(result) {

        mostrarCargando(false);

        var nombre = result.Area.Nombre;
        $('#inputFormulario_Nombre').val(nombre);

        var observaciones = result.Area.Observaciones;
        $('#inputFormulario_Observaciones').val(observaciones);

        var fechaBaja = result.Area.FechaBaja;

        if (fechaBaja == null) {
            $('#rdbActivoSi').prop("checked", true);
        }
        else {
            $('#rdbActivoNo').prop("checked", true);
        }
        $('#contenedorEstado').show();

        Materialize.updateTextFields();
    }
    function onError(result) {
        alert(result.get_message());
    }
}


function setOnAreaGuardadoListener(callback) {
    this.callbackGuardar = callback;
}


function setOnAreaEditadoListener(callback) {
    this.callbackEditar = callback;
}

////-------------------------------
//// Cargando
////-------------------------------

//function mostrarCargando(mostrar, mensaje) {
//    if (callbackCargando != undefined) {
//        callbackCargando(mostrar, mensaje);
//    }
//}

//function setOnCargandoListener(callback) {
//    this.callbackCargando = callback;
//}

////-----------------------------
//// Alertas
////-----------------------------

//function setOnMensajeListener(callback) {
//    this.callbackMensaje = callback;
//}

//function mostrarMensajeError(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Error', mensaje);
//}

//function mostrarMensajeAlerta(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Alerta', mensaje);
//}

//function mostrarMensajeInfo(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Info', mensaje);
//}