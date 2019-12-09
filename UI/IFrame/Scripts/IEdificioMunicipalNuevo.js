var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;

var categorias = [];

var callback;

var edificioMunicipal;

function init(data) {
    data = parse(data);

    if (data.Categorias != undefined) {
        funciones = data.Categorias;
    }

    ControlDomicilioSelector_Init({ MostrarEdificiosMunicipales: false });

    if (data.Categorias != 0) {
        $("#select_Categoria").CargarSelect({
            Data: data.Categorias,
            Value: 'Id',
            Text: 'Nombre',
            Sort: false
        });
    }

    if (data.IdCategoria != undefined) {
        $("#select_Categoria").val(data.IdCategoria).trigger('change')
    }

    if (data.EdificioMunicipal != undefined) {
        modo = modoEditar;
        edificioMunicipal = data.EdificioMunicipal;
        setEntity(edificioMunicipal);
    }

}

//-----------------------------
// Operaciones globales 
//-----------------------------
function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando Edificio Municipal...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/EdificioMunicipalService.asmx/Insert'),
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

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando origen...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/EdificioMunicipalService.asmx/Editar'),
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

    //Nombre
    var nombre = $('#input_Nombre').val();
    if (nombre == undefined || nombre == "") {
        mostrarMensaje("Info", "Faltan datos obligatorios");
        $('#input_Nombre').siblings('.control-observacion').text('Dato requerido');
        resultado = false;
    }

    if (!ControlDomcilioSelector_HayDomicilioSeleccionado() ) {
        mostrarMensaje("Info", "Debe ingresar un domicilio");
        resultado = false;
    }

    return resultado;
}

function getEntity() {
    var entity = {};

    if (modo == modoEditar) {
        entity.Id = edificioMunicipal.Id;
    }

    //Ubicacion
    if (ControlDomcilioSelector_HayDomicilioSeleccionado()) {
        entity.Domicilio = ControlDomicilioSelector_GetDomicilio();
    } else {
        entity.Domicilio = null;
    }

    entity.Nombre = $('#input_Nombre').val();
    entity.IdCategoria = $('#select_Categoria').val();
    return entity;
}

function setEntity(entity) {
    $('#input_Nombre').val(entity.Nombre);
    entity.IdCategoria = $('#select_Categoria').val(entity.IdCategoria).trigger("change");
    if (entity.Domicilio != null) {
        ControlDomicilioSelector_CargarUbicacionSeleccionada(entity.Domicilio);
    }
    Materialize.updateTextFields();
}

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}