function init(data) {
    ControlMapa_Init({
        Buscar: true,
        ResaltarAlHacerClick: true
    });
}


function onMapReady(map) {

}

function validarLimpiarMapa() {

}

function onMapaLimpiado() {

}

function onMapaClick(event) {
    //console.log('click');
    //console.log(event);
}


//-------------------------------------------
// Resaltar al hacer click
//-------------------------------------------

function validarResaltarAlHacerClick(event) {
}


//Marcador
function validarMostrarMarcador(data) {
}

function onMarcadorAgregado(marcador, data) {

}

function crearMarcador(data) {
    //let color = '#000000';

    //return {
    //    path: 'M31.5,0C14.1,0,0,14,0,31.2C0,53.1,31.5,80,31.5,80S63,52.3,63,31.2C63,14,48.9,0,31.5,0z M31.5,52.3 c-11.8,0-21.4-9.5-21.4-21.2c0-11.7,9.6-21.2,21.4-21.2s21.4,9.5,21.4,21.2C52.9,42.8,43.3,52.3,31.5,52.3z',
    //    fillColor: color,
    //    fillOpacity: 1,
    //    anchor: new google.maps.Point(35, 70),
    //    strokeColor: '#000',
    //    strokeWeight: 2,
    //    scale: 0.45,
    //};
}

function crearPopupMarcador(data) {
    //return '<label>Hola</label>';
}


//CPC
function validarResaltarCpc(id) {
    //return true;
}

function onCpcResaltado(id, poligono) {

}


//Barrio
function validarResaltarBarrio(id) {
    //return true;
}

function onBarrioResaltado(id, poligono) {

}


//-------------------------------------------
// Busqueda
//-------------------------------------------

function validarInputBusquedaKeyDown(key, texto) {

}

function onInputBusquedaChange(texto) {

}

function validarConsulta(consulta) {

    //return 'hola';
}

function validarSugerencias(sugerencias) {
    //return [];
}

function onSugerenciasObtenidas(sugerencias) {

}
