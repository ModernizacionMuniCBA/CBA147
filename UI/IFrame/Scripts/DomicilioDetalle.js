var domicilio;

var callbackMensaje;
var callbackCargando;

function init(data) {
    data = parse(data);

    domicilio = data.Domicilio;
    cargarDatos();
}

function mostrarMensajeError(mensaje) {

}

function cargarDatos() {
    
    //La calle solo si es por calle
    if ('Calle' in domicilio && domicilio.Calle !=null) {
        $('#contenedorCalle').show();
        $('#textoDireccion').text(toTitleCase(domicilio.Calle.Nombre) + ' ' + domicilio.Altura);
    } else {
        $('#contenedorCalle').hide();
    }

    //El barrio siempre
    $('#textoBarrio').text(toTitleCase(domicilio.Barrio.Nombre));

    //CPC solo si tiene
    if ('CPC' in domicilio && domicilio.CPC != null) {
        $('#contenedorCPC').show();
        $('#textoCPC').text(toTitleCase(domicilio.CPC.Nombre));
    } else {
        $('#contenedorCPC').hide();
    }

    //Observaciones
    if (domicilio.Observaciones != null && domicilio.Observaciones != "") {
        $('#contenedorObservaciones').show();
        $('#textoObservaciones').text(domicilio.Observaciones);
    } else {
        $('#contenedorObservaciones').hide();
    }
}

function verMapa() {

}
