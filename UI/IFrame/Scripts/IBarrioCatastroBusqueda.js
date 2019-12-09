var callbackSeleccionar;

$(function () {
    SelectorDomicilio_SetModo(SelectorDomicilio_ModoBarrio);
    $('#SelectorDomicilio_ContenedorBusqueda > div.row').hide()

    SelectorDomicilio_SetOnDomicilioSeleccionadoListener(function (domicilio) {
        informar(domicilio.Barrio);
    });
});

function informar(barrio) {
    if (callbackSeleccionar == undefined) return;
    callbackSeleccionar(barrio);
}

function setCallback(callback) {
    callbackSeleccionar = callback;
}