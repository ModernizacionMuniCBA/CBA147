let MODO_EDITAR = 'editar';
let MODO_REGISTRAR = 'regitrar';
let modo;

let servicio;
let callback;

function init(data) {
    if ('Error' in data) {
        mostrarMensajeCritico({});
        return;
    }

    if (data.Servicio != undefined) {
        modo = MODO_EDITAR;
        servicio = data.Servicio;

        cargarDatos();
    } else {
        modo = MODO_REGISTRAR;
    }

}

function cargarDatos() {
    $('#input_Nombre').val(servicio.Nombre);
    $('#input_Descripcion').val(servicio.Observaciones);
    $('#input_Icono').val(servicio.Icono);
    $('#input_UrlIcono').val(servicio.UrlIcono);
    $('#input_Color').val(servicio.Color);
    $('#check_Principal').prop('checked', servicio.Principal);
    Materialize.updateTextFields();
}

function validar() {
    let motivo = getServicio();

    if (motivo.Nombre == "") {
        mostrarMensajeError('Ingrese el nombre');
        return false;
    }

    return true;
}

function registrar() {
    if (!validar()) return;

    mostrarCargando(true);
    ajax_Registrar(getServicio())
        .then(function (data) {
            informar();
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarMensajeError(error);
        });
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true);
    ajax_Editar(getServicio())
        .then(function (data) {
            informar();
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarMensajeError(error);
        });
}

function getServicio() {
    let data = {};

    if (modo == MODO_EDITAR) {
        data.Id = servicio.Id;
    }

    data.Nombre = $('#input_Nombre').val();
    data.Descripcion = $('#input_Descripcion').val();
    data.Icono = $('#input_Icono').val();
    data.UrlIcono = $('#input_UrlIcono').val();
    data.Color = $('#input_Color').val();
    data.Principal = $('#check_Principal').is(':checked');
    return data;
}

function ajax_Registrar(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioService.asmx/Insertar'),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}

function ajax_Editar(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioService.asmx/Editar'),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}

//-------------------------------
// Registrar
//-------------------------------

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}

function informar() {
    if (callback == undefined) return;
    callback();
}

