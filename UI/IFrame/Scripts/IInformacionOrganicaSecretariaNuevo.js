let MODO_EDICION = 'editar';
let MODO_NUEVO = 'nuevo';

let modo;
let secretaria;

let callback;

function init(data) {
    secretaria = data.Secretaria;
    if (secretaria != undefined) {
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
    $('#input_Nombre').val(secretaria.Nombre);
}

function validar() {
    return $('#form').valid();
}

function registrar() {
    if (!validar()) return;

    mostrarCargando(true);

    var url = ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/Insertar');

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

            mostrarMensaje('Exito', "Secretaria registrada correctamente");
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

    var url = ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/Actualizar');

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

            mostrarMensaje('Exito', "Secretaria editada correctamente");
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
        data.Id = secretaria.Id;
    }
    data.Nombre = $('#input_Nombre').val();
  
    return data;
}




function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}