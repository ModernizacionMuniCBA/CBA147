
var origen;

function init(data) {
    origen = data.Origen;
    cargarDatos();
}

function cargarDatos() {
    $('#textoNombre').text(origen.Nombre);
    $('#textoKeyAlias').text(origen.KeyAlias);
    $('#textoKeySecret').text(origen.KeySecret);

    Materialize.updateTextFields();
}
