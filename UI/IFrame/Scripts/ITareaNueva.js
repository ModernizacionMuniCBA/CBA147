var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;
var tarea;
var idArea;

var callback;

function init(data) {
    //--------------------------
    // Init Data
    //--------------------------
    data = parse(data);

    //----------------------------
    // MODO
    //----------------------------

    tarea = data.Tarea;
    idArea = data.IdArea;
    if (tarea == undefined) {
        modo = modoRegistrar;
        $("#contenedorEstado").hide();

    } else {
        modo = modoEditar;
        if (tarea == null) {
            mostrarMensaje('Error', 'Error inicializando la pantalla');
            return;
        }

        $('#inputFormulario_Nombre').val(toTitleCase(tarea.Nombre));
        $('#inputFormulario_Observaciones').val(tarea.Observaciones);
        $('#contenedorEstado').show();

        var fechaBaja = tarea.FechaBaja;

        if (fechaBaja == null) {
            $('#rdbActivoSi').prop("checked", true);
        }
        else {
            $('#rdbActivoNo').prop("checked", true);
        }
        Materialize.updateTextFields();
    }
    setTimeout(function () {
        mostrarCargando(false);
    }, 600)
}

function errorInit() {
    mostrarMensaje('Error', 'Error inicializando la pagina');
}

//--------------------------
// Operacion principal
//--------------------------

function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando Tarea...');

    var data = getTarea();

    crearAjax({
        Data: { comando: data },
        Url: ResolveUrl('~/Servicios/TareaService.asmx/Insertar'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.Return == null) {
                mostrarMensaje('Error', 'Error registrando la tarea');
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.statusText);
        }
    });

}

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando Tarea...');

    var dataAjax = getTarea();

    crearAjax({
        Data: { comando: dataAjax },
        Url: ResolveUrl('~/Servicios/TareaService.asmx/Update'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.Return == null) {
                mostrarMensaje('Error', 'Error editando la tarea');
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.statusText);
        }
    });

}

function validar() {
    $('.control-observacion').text('');

    var todoOk = true;

    //Nombre
    var nombre = $('#inputFormulario_Nombre').val();
    if (nombre == undefined || nombre == "") {
        $('#inputFormulario_Nombre').siblings('.control-observacion').text('Dato requerido');
        todoOk = false;
    }

    return todoOk;
}

function getTarea() {

    var t = {};
    if (modo == modoEditar) {
        t.Id = "" + tarea.Id;
    }

    t.Nombre = "" + $('#inputFormulario_Nombre').val();
    t.Observaciones = "" + $('#inputFormulario_Observaciones').val();

    //Dado o no de alta
    if ($('#contenedorEstado').lenght != 0) {
        if ($('#rdbActivoNo').is(":checked")) {
            t.Activo = false;
        } else {
            t.Activo = true;
        }
    } else {
        t.Activo = tarea.FechaBaja == null;
    }

    //Area
    t.IdArea = idArea;

    return t;
}


//-------------------------------
// Registrar
//-------------------------------

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined || callback == null) return;
    callback(entity);
}
