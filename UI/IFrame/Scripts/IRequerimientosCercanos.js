var requerimientos;
var motivo;
var marcadores;
var centro;

var map;
var callbackReady;
var ZOOM_UN_MARCADOR = 17;
var MAX_ZOOM = 19;
var informado = false;

var callbackClickMarcador;
var marcadoresMapa = [];

var tabla;

function init(data) {
    data = parse(data);

    requerimientos = data.Requerimientos.Data;
    motivo = data.Motivo;

    centro = {
        lat: parseFloat(data.Latitud.replace(',', '.')),
        lng: parseFloat(data.Longitud.replace(',', '.'))
    };
    marcadores = data.Marcadores;

    inicializar();
}

function inicializar() {
    initHeader();
    initTabla();
    initMapa();
}


function initHeader() {

}

function initTabla() {
    tabla = $('#tabla').DataTableReclamo2({
        //Callbacks generales
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (cargando, mensaje) {
            mostrarCargando(cargando, mensaje);
        },
        //Editar
        BotonEditar: true,
        CallbackEditar: function (rq) {
            actualizarRequerimientoEnGrilla(rq);
        },
        //Estado
        BotonCambiarEstado: true,
        CallbackCambiarEstado: function (rq) {
            actualizarRequerimientoEnGrilla(rq);
        },
        //Mensaje
        BotonEnviarMensaje: true,
        //Cancelar
        BotonCancelar: true,
        CallbackCancelar: function (rq) {
            actualizarRequerimientoEnGrilla(rq);
        },
        //Mail
        BotonEnviarMail: true,
        //Imprimir
        BotonImprimir: true,
        //Imprimir Sin Mapa
        BotonImprimirSinMapa: true,
        VerDomicilio: false,
        BotonUnirseARequerimiento: true,
        CallbackUnirseARequerimiento: function (id) {
            informarReady(id);
        }
    });

    //Inicializo los tooltips
    tabla.$('.tooltipped').tooltip({ delay: 50 });
    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function initMapa() {

    $.getScript("https://maps.googleapis.com/maps/api/js?key=" + KEY_GOOGLE_MAPS + "&libraries=visualization", function () {

        var myStyles = [
            {
                featureType: "poi",
                elementType: "labels",
                stylers: [
                      { visibility: "off" }
                ]
            }
        ];

        map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: parseFloat(centro.lat), lng: parseFloat(centro.lng) },
            zoom: 8,
            maxZoom: MAX_ZOOM,
            fullscreenControl: false,
            gestureHandling: 'greedy',
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
                position: google.maps.ControlPosition.TOP_RIGHT
            },
            styles: myStyles
        });


        mover(centro.lat, centro.lng);
        map.setZoom(15);

        var radioMin = 1;
        var radioMax = 300;
        var alphaMin = 0;
        var alphaMax = 0.2;
        var direction = 1;

        var circulo = new google.maps.Circle({
            strokeColor: '#FF0000',
            strokeOpacity: 0,
            strokeWeight: 0,
            fillColor: '#000000',
            fillOpacity: alphaMax,
            map: map,
            center: { lat: centro.lat, lng: centro.lng },
            radius: radioMin
        });


        setInterval(function () {
            var radio = circulo.getRadius();
            radio += 1;
            if (radio > radioMax) {
                radio = radioMin;
            }
            circulo.setRadius(radio);

            var alpha = (1 - (radio / radioMax)) * alphaMax;
            if (alpha > alphaMax) alpha = alphaMax;
            if (alpha < alphaMin) alpha = alphaMin;
            circulo.setOptions({ fillOpacity: alpha });
        }, 7);


        cargarDatos();
    });
}

function centrarEnCordoba(callback) {
    var geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': 'Cordoba, Argentina' }, function (results, status) {
        var ne = results[0].geometry.viewport.getNorthEast();
        var sw = results[0].geometry.viewport.getSouthWest();

        map.fitBounds(results[0].geometry.viewport);

        callback();
    });
}

function animarMarcador(idRequerimiento) {
    var marcador = $.grep(marcadores, function (element, index) {
        return element.IdRequerimiento == idRequerimiento;
    });

    if (marcador == undefined || marcador.length == 0) return;

    var marcador = $.grep(marcadoresMapa, function (element, index) {
        return element.get('id') == marcador[0].Id;
    });

    marcador = marcador[0];
    if (marcador == undefined) return;
    marcador.setAnimation(google.maps.Animation.BOUNCE);
}

function quitarAnimacionMarcador(idRequerimiento) {
    var marcador = $.grep(marcadores, function (element, index) {
        return element.IdRequerimiento == idRequerimiento;
    });

    if (marcador == undefined || marcador.length == 0) return;

    var marcador = $.grep(marcadoresMapa, function (element, index) {
        return element.get('id') == marcador[0].Id;
    });

    marcador = marcador[0];
    if (marcador == undefined) return;
    marcador.setAnimation(null);
}

function crearHtmlPopup(id, titulo, descripcion, callback) {
    if (titulo == undefined || titulo == "") return undefined;

    var html = $('<div class="popup">');

    var divTextos = $('<div class="textos">');
    $(divTextos).appendTo(html);

    var labelTitulo = $('<label class="titulo">');
    $(labelTitulo).text(titulo);
    $(labelTitulo).appendTo(divTextos);

    if (descripcion != undefined) {
        var labelDescripcion = $('<label class="descripcion">');
        $(labelDescripcion).text(descripcion);
        $(labelDescripcion).appendTo(divTextos);
    }

    if (callback != undefined) {
        var btn = $('<a class="btn waves-effect btn-cuadrado" onclick="clickBotonDetalle(' + id + ')"><i class="material-icons btn-icono">description</i></a>');
        $(btn).appendTo(html);
    }

    return html;
}

function clickBotonDetalle(id) {
    if (callbackClickMarcador == undefined) return;
    callbackClickMarcador(id);
}


function cargarDatos() {
    //Cargo la tabla
    cargarTabla();
    cargarMarcadores();
}

function cargarHeader() {

}

function cargarTabla() {
    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (requerimientos != null && requerimientos.length != 0) {
        dt.rows.add(requerimientos).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Cantidad de rows
    calcularCantidadDeRows();
}

function cargarMarcadores() {
    //Cargo los marcadores
    $.each(marcadores, function (index, element) {

        let lat = parseFloat(element.Latitud.replace(',', '.'));
        let lng = parseFloat(element.Longitud.replace(',', '.'));

        var marcador = { lat: lat, lng: lng };

        var marker = new google.maps.Marker({
            position: marcador,
            map: map
        });
        marcadoresMapa.push(marker);

        marker.setIcon(pinSymbol('#' + element.EstadoColor));
        marker.set("id", element.Id);

        callbackClickMarcador = function (id) {
            crearDialogoDetalleRequerimiento({
                Id: id
            });
        };

        var html = crearHtmlPopup(element.Id, element.Numero + '/' + element.Año, '', callbackClickMarcador);

        if (html != undefined) {

            var infowindow = new google.maps.InfoWindow({
                content: $(html).prop('outerHTML'),
                maxWidth: 400
            });

            marker.addListener('click', function () {
                infowindow.open(map, marker);
            });
        }
    });

    $('.contenedorTabla').on('mouseenter', "[role='row']", function () {
        var data = tabla.row($(this)).data();
        if (data == undefined) return;
        animarMarcador(data.Id);
    });

    $('.contenedorTabla').on('mouseleave', "[role='row']", function () {
        var data = tabla.row($(this)).data();
        if (data == undefined) return;
        quitarAnimacionMarcador(data.Id);
    });
}


/* Utiles */

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').find('#tabla_wrapper').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();
}

function actualizarRequerimientoEnGrilla(rq) {
    //Busco el indice de la persona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == rq.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(rq);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function pinSymbol(color) {
    return {
        path: 'M31.5,0C14.1,0,0,14,0,31.2C0,53.1,31.5,80,31.5,80S63,52.3,63,31.2C63,14,48.9,0,31.5,0z M31.5,52.3 c-11.8,0-21.4-9.5-21.4-21.2c0-11.7,9.6-21.2,21.4-21.2s21.4,9.5,21.4,21.2C52.9,42.8,43.3,52.3,31.5,52.3z',
        fillColor: color,
        fillOpacity: 1,
        anchor: new google.maps.Point(35, 70),
        strokeColor: '#000',
        strokeWeight: 2,
        scale: 0.45,
    };
}


/* Api global */

function imprimir() {

    var ids = [];

    var dt = $('#tabla').DataTable();
    $.each(dt.rows().data(), function (index, row) {
        ids.push(parseInt(row.Id));
    });

    var creado = crearDialogoReporteRequerimientoListado({
        Ids: ids,
        CallbackCargando: function (cargando, mensaje) {
            callbackCargando(cargando, mensaje)
        }
    })

    if (creado == false) {
        mostrarMensajeError('Error imprimiendo');
    }
}

function mover(x, y) {
    if (map == undefined) return;


    var center = new google.maps.LatLng(x, y);
    console.log('x: ' + x);
    console.log('y: ' + y);

    map.panTo(center);
}


/* Listener */

function setOnReadyListener(callback) {
    callbackReady = callback;
}

function informarReady(id) {
    if (callbackReady == undefined) return;
    callbackReady(id);
}
