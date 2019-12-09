let callbackSeleccionar;

$(function () {

    //Seleccion
    RequerimientoSelector_SetOnRequerimientoSeleccionadoListener(function (rq) {
        informarSeleccion(rq);
    });

    //Cargando
    RequerimientoSelector_SetOnCargandoListener(function (mostrar, mensaje) {
        mostrarCargando(mostrar, mensaje);
    });

    //Mensajes
    RequerimientoSelector_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    })
});

function focus(){
    RequerimientoSelector_Focus();
}

//-------------------------------
// Listener
//-------------------------------

function informarSeleccion(rq) {
    if (callbackSeleccionar == null || callbackSeleccionar == undefined) return;
    callbackSeleccionar(rq);
}

function setOnRequerimientoSeleccionadoListener(callback) {
    callbackSeleccionar = callback;
}

function isRequerimientoSeleccionado() {
    return RequerimientoSelector_IsRequerimientoSeleccionado();
}

function getRequerimientoSeleccionado() {
    return RequerimientoSelector_GetRequerimientoSeleccionado();
}
