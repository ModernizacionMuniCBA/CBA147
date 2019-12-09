let MODO_EDICION = 'editar';
let MODO_NUEVO = 'nuevo';

let modo;
let direccion;
let secretarias;

let callback;

function init(data) {
    direccion = data.Direccion;
    secretarias = data.Secretarias;

    $("#input_Secretaria").CargarSelect({
        Data: secretarias,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    let idSecretaria = $.url().param('IdSecretaria');
    if (idSecretaria != undefined) {
        $("#input_Secretaria").val(idSecretaria).trigger('change');
        $("#contenedor_Secretaria").hide();
    }

    if (direccion != undefined) {
        modo = MODO_EDICION;
    } else {
        modo = MODO_NUEVO;
    }


    switch (modo) {
        case MODO_NUEVO: {

        } break;

        case MODO_EDICION: {
            cargarEntidad();
        } break;
    }

    $('#form').on('submit', function () {
        switch (modo) {
            case MODO_NUEVO: {
                registrar();
            } break;

            case MODO_EDICION: {
                editar();
            } break;
        }
        return false;
    });
}

function cargarEntidad() {
    $('#input_Secretaria').val(direccion.Secretaria.Id).trigger('change');

    $('#input_Nombre').val(direccion.Nombre);
    $('#input_Responsable').val(direccion.Responsable);
    $('#input_Domicilio').val(direccion.Domicilio);
    $('#input_Telefono').val(direccion.Telefono);
    $('#input_Email').val(direccion.Email);
    $('#input_Observaciones').val(direccion.Observaciones);
}

function validar() {
    return $('#form').valid();
}

function registrar() {
    if (!validar()) return;

    mostrarCargando(true);

    var url = ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/Insertar');

    crearAjax({
        Url: url,
        Data: { comando: getData() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "Dirección registrada correctamente");
            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true);

    var url = ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/Actualizar');

    crearAjax({
        Url: url,
        Data: { comando: getData() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "Dirección editada correctamente");
            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}

function getData() {
    let data = {};
    if (modo == MODO_EDICION) {
        data.Id = direccion.Id;
    }
    data.Nombre = $('#input_Nombre').val();
    data.Responsable = $('#input_Responsable').val();
    data.Domicilio = $('#input_Domicilio').val();
    data.Telefono = $('#input_Telefono').val();
    data.Email = $('#input_Email').val();
    data.IdSecretaria = $('#input_Secretaria').val();
    return data;
}

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}