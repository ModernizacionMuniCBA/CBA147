var callbackMensaje;
var callbackCargando;

var quill;

var modoEditar = "editar";
var modoRegistrar = "registrar";

var modo;
var notificacion;

function parse(json) {
    if (json == null || json == undefined || json == "" || typeof json != 'string') {
        return json;
    }
    ////json = json.replace(/\\n/g, "\\n")
    ////           .replace(/\\'/g, "\\'")
    ////           .replace(/\\"/g, '\\"')
    ////           .replace(/\\&/g, "\\&")
    ////           .replace(/\\r/g, "\\r")
    ////           .replace(/\\t/g, "\\t")
    ////           .replace(/\\b/g, "\\b")
    ////           .replace(/\\f/g, "\\f");
    ////json = json.replace(/[\u0000-\u0019]+/g, "");

    try {
        json = JSON.parse(json);
    } catch (e) {
        json = json.replace(/\\/g, "");
        json = JSON.parse(json);
    }
    return json;
}


function init(data) {

    quill = new Quill('#editor', {
        modules: {
            toolbar: [
              [{ 'size': ['small', false, 'large', 'huge'] }],
              [{ 'color': [] }, { 'background': [] }],
              ['bold', 'italic'],
              [{ list: 'ordered' }, { list: 'bullet' }, { 'align': [] }, 'blockquote', 'code-block'],
              ['link', 'image'],
              ['clean']
            ]
        },
        placeholder: 'Inserte el contenido de su notificación...',
        theme: 'snow'
    });

    if ('Notificacion' in data && data.Notificacion != undefined) {
        modo = modoEditar;
        initModoEditar(data);
    } else {
        modo = modoRegistrar;
        initModoRegistrar(data);
    }

}

function initModoRegistrar(data) {

}

function initModoEditar(data) {
    notificacion = data.Notificacion;
    $('#input_Titulo').val(notificacion.Titulo);
    $('.ql-editor').html(notificacion.Contenido);
    $('#check_Notificar').prop('checked', notificacion.Notificar);
}


function registrar() {
    if (!validar()) return;

    mostrarCargando(true, 'Registrando');
    crearAjax({
        Url: ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/Insertar'),
        Data: { comando: getComandoRegistrar() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensajeError(result.Error);
                return;
            }

            //Informo
            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensajeError('Error procesando la solicitud');
        }
    });
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true, 'Registrando');
    crearAjax({
        Url: ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/Editar'),
        Data: { comando: getComandoRegistrar() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensajeError(result.Error);
                return;
            }

            //Informo
            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensajeError('Error procesando la solicitud');
        }
    });
}

function validar() {
    if ($('#input_Titulo').val() == undefined || $('#input_Titulo').val().trim() == "") {
        mostrarMensajeAlerta('Debe ingresar el titulo');
        $('#input_Titulo').focus();
        return false;
    }

    return true;
}

function getComandoRegistrar() {
    var comando = {};

    if (notificacion != undefined) {
        comando.Id = notificacion.Id;
    }
    comando.Titulo = $('#input_Titulo').val();
    comando.Contenido = $('.ql-editor').html();
    comando.Notificar = $('#check_Notificar').is(':checked');
    return comando;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}


function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}

//-------------------------------
// Listener Cargando
//-------------------------------

function mostrarCargando(mostrar, mensaje) {
    if (callbackCargando != undefined) {
        callbackCargando(mostrar, mensaje);
    }
}

function setOnCargandoListener(callback) {
    this.callbackCargando = callback;
}

//-----------------------------
// Listener Alertas
//-----------------------------

function setOnMensajeListener(callback) {
    this.callbackMensaje = callback;

}

function mostrarMensaje(tipo, mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje(tipo, mensaje);
}

function mostrarMensajeError(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Error', mensaje);
}

function mostrarMensajeExito(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Exito', mensaje);
}

function mostrarMensajeAlerta(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Alerta', mensaje);
}

function mostrarMensajeInfo(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Info', mensaje);
}
